using Bank.Application.Domain;
using Bank.UserService.Models;
using Bank.UserService.Repositories;

namespace Bank.UserService.HostedServices;

public class LoanHostedService
{
    private readonly IServiceProvider           _serviceProvider;
    private readonly ILogger<LoanHostedService> _logger;
    private          Timer?                     _timer;
    private readonly Random                     _random = new Random();

    public LoanHostedService(IServiceProvider serviceProvider, ILogger<LoanHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger          = logger;
    }

    public void OnApplicationStarted()
    {
        Initialize();
    }

    public void OnApplicationStopped()
    {
        _timer?.Dispose();
    }

    private void Initialize()
    {
        var midnight          = DateTime.Today.AddDays(1);
        var timeLeftUntilNext = midnight.Subtract(DateTime.UtcNow);

        _logger.LogInformation($"Loan payment processing scheduled to start in {timeLeftUntilNext}");
        _timer = new Timer(async _ => await ProcessLoanPayments(), null, timeLeftUntilNext, TimeSpan.FromDays(1));
    }

    private async Task ProcessLoanPayments()
    {
        _logger.LogInformation("Starting scheduled loan payment processing");

        try
        {
            using var scope                 = _serviceProvider.CreateScope();
            var       loanRepository        = scope.ServiceProvider.GetRequiredService<ILoanRepository>();
            var       accountRepository     = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
            var       installmentRepository = scope.ServiceProvider.GetRequiredService<IInstallmentRepository>();

            var today = DateTime.UtcNow.Date;

            var activeLoans = await loanRepository.GetLoansWithDueInstallmentsAsync(today);
            _logger.LogInformation($"Found {activeLoans.Count} loans with due installments");

            foreach (var loan in activeLoans)
            {
                await ProcessLoanInstallmentsAsync(loan, today, loanRepository, accountRepository, installmentRepository);
            }

            _logger.LogInformation("Completed loan payment processing");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during loan payment processing");
        }
    }

    private async Task ProcessLoanInstallmentsAsync(Loan                   loan, DateTime currentDate, ILoanRepository loanRepository, IAccountRepository accountRepository,
                                                    IInstallmentRepository installmentRepository)
    {
        try
        {
            // Get all pending installments due today or earlier
            var dueInstallments = await installmentRepository.GetDueInstallmentsForLoanAsync(loan.Id, currentDate);

            if (dueInstallments.Count == 0)
                return;

            _logger.LogInformation($"Processing {dueInstallments.Count} installments for loan {loan.Id}");

            foreach (var installment in dueInstallments)
            {
                // Calculate payment amount for this installment
                decimal paymentAmount = await CalculateInstallmentAmount(loan, installment, installmentRepository);
                _logger.LogInformation($"Calculated payment amount: {paymentAmount} for installment {installment.Id}");

                // Process payment
                bool paymentSuccessful = await ProcessPaymentAsync(loan, installment, paymentAmount, accountRepository);

                if (paymentSuccessful)
                {
                    // Update the installment
                    var updatedInstallment = new Installment
                                             {
                                                 Id              = installment.Id,
                                                 LoanId          = installment.LoanId,
                                                 InterestRate    = installment.InterestRate,
                                                 ExpectedDueDate = installment.ExpectedDueDate,
                                                 ActualDueDate   = currentDate,
                                                 Status          = InstallmentStatus.Paid,
                                                 CreatedAt       = installment.CreatedAt,
                                                 ModifiedAt      = DateTime.UtcNow
                                             };

                    await installmentRepository.Update(installment, updatedInstallment);
                    _logger.LogInformation($"Installment {installment.Id} marked as paid");

                    // Create next installment if needed
                    await CreateNextInstallmentIfNeededAsync(loan, installment, loanRepository, installmentRepository);
                }
                else
                {
                    _logger.LogWarning($"Payment processing failed for installment {installment.Id}");
                }
            }

            // Check if loan is complete
            await CheckLoanCompletionAsync(loan, loanRepository, installmentRepository);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing installments for loan {loan.Id}");
        }
    }

    /*private async Task<bool> ProcessPaymentAsync(Loan loan, Installment installment, decimal paymentAmount, IAccountRepository accountRepository)
    {
        try
        {
            // Get the account associated with the loan
            var account = await accountRepository.FindById(loan.AccountId);

            if (account == null)
            {
                _logger.LogWarning($"Account {loan.AccountId} not found for loan {loan.Id}");
                return false;
            }

            // Check if account has sufficient funds
            if (account.Balance < paymentAmount)
            {
                _logger.LogWarning($"Insufficient funds in account {account.Id} for loan payment. Available: {account.Balance}, Required: {paymentAmount}");
                return false;
            }

            // TODO: This should be done with transactions
            // Update account balance
            // var updatedAccount = new Account
            // {
            //     Id = account.Id,
            //     CustomerId = account.CustomerId,
            //     AccountNumber = account.AccountNumber,
            //     Balance = account.Balance - paymentAmount,
            //     Type = account.Type,
            //     CurrencyId = account.CurrencyId,
            //     BranchId = account.BranchId,
            //     Status = account.Status,
            //     CreatedAt = account.CreatedAt,
            //     ModifiedAt = DateTime.UtcNow
            // };
            //
            // await accountRepository.Update(account, updatedAccount);

            _logger.LogInformation($"Payment of {paymentAmount} {loan.Currency.Code} processed for loan {loan.Id}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing payment for loan {loan.Id}, installment {installment.Id}");
            return false;
        }
    }*/
    
    private async Task<bool> ProcessPaymentAsync(Loan loan, Installment installment, decimal paymentAmount, IAccountRepository accountRepository)
{
    try
    {
        // Get the account associated with the loan
        var account = await accountRepository.FindById(loan.AccountId);

        if (account == null)
        {
            _logger.LogWarning($"Account {loan.AccountId} not found for loan {loan.Id}");
            return false;
        }

        // Check if account has sufficient funds
        if (account.AvailableBalance < paymentAmount)
        {
            _logger.LogWarning($"Insufficient funds in account {account.Id} for loan payment. Available: {account.AvailableBalance}, Required: {paymentAmount}");
            return false;
        }

        // Get necessary repositories
        using var scope = _serviceProvider.CreateScope();
        var transactionRepo = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
        var transactionCodeRepo = scope.ServiceProvider.GetRequiredService<ITransactionCodeRepository>();
        
        // Get transaction code for loan payment
        // Option 1: If you know the ID of the loan payment code
        /*var loanPaymentCodeId = new Guid("known-guid-for-loan-payment-code"); // Replace with actual ID
        var loanPaymentCode = await transactionCodeRepo.FindById(loanPaymentCodeId);*/
        
        var allCodes = await transactionCodeRepo.FindAll(new Pageable());  
        var loanPaymentCode = allCodes.Items.FirstOrDefault(c => c.Code == "289");
        
        if (loanPaymentCode == null)
        {
            _logger.LogWarning("Loan payment transaction code not found");
            return false;
        }

        // Create transaction for the loan payment
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            FromAccountId = account.Id,
            ToAccountId = null, 
            FromAmount = paymentAmount,
            ToAmount = paymentAmount,
            FromCurrencyId = loan.CurrencyId,
            ToCurrencyId = loan.CurrencyId,
            CodeId = loanPaymentCode.Id,
            ReferenceNumber = $"LOAN-{loan.Id}-INST-{installment.Id}",
            Purpose = $"Loan payment for loan #{loan.Id}, installment #{installment.Id}",
            Status = TransactionStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        // Use transaction scope to ensure atomicity
        using var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled);
        
        try
        {
            // Update account balance
            var updatedAccount = new Account
            {
                Id = account.Id,
                ClientId = account.ClientId,
                Name = account.Name,
                Number = account.Number,
                Balance = account.Balance - paymentAmount,
                AvailableBalance = account.AvailableBalance - paymentAmount,
                EmployeeId = account.EmployeeId,
                CurrencyId = account.CurrencyId,
                AccountTypeId = account.AccountTypeId,
                AccountCurrencies = account.AccountCurrencies,
                DailyLimit = account.DailyLimit,
                MonthlyLimit = account.MonthlyLimit,
                CreationDate = account.CreationDate,
                ExpirationDate = account.ExpirationDate,
                Status = account.Status,
                CreatedAt = account.CreatedAt,
                ModifiedAt = DateTime.UtcNow
            };
            
            await accountRepository.Update(account, updatedAccount);
            
            // Save the transaction
            await transactionRepo.Add(transaction);
            
            // Complete the transaction scope
            transactionScope.Complete();
            
            _logger.LogInformation($"Payment of {paymentAmount} {loan.Currency?.Code} processed for loan {loan.Id}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating accounts during loan payment processing");
            return false;
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error processing payment for loan {loan.Id}, installment {installment.Id}");
        return false;
    }
}

    private async Task CreateNextInstallmentIfNeededAsync(Loan loan, Installment currentInstallment, ILoanRepository loanRepository, IInstallmentRepository installmentRepository)
    {
        int totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);

        if (totalInstallments < loan.Period)
        {
            var latestInstallment = await installmentRepository.GetLatestInstallmentForLoanAsync(loan.Id);

            // Create next month's installment
            var newInstallment = new Installment
                                 {
                                     Id              = Guid.NewGuid(),
                                     LoanId          = loan.Id,
                                     InterestRate    = GetEffectiveInterestRate(loan, currentInstallment),
                                     ExpectedDueDate = latestInstallment.ExpectedDueDate.AddMonths(1),
                                     Status          = InstallmentStatus.Pending,
                                     CreatedAt       = DateTime.UtcNow,
                                     ModifiedAt      = DateTime.UtcNow
                                 };

            await installmentRepository.Add(newInstallment);
            _logger.LogInformation($"Created next installment for loan {loan.Id} due on {newInstallment.ExpectedDueDate}");
        }
    }

    private async Task CheckLoanCompletionAsync(Loan loan, ILoanRepository loanRepository, IInstallmentRepository installmentRepository)
    {
        bool allPaid           = await installmentRepository.AreAllInstallmentsPaidAsync(loan.Id);
        int  totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);

        if (allPaid && totalInstallments >= loan.Period)
        {
            // Mark loan as completed
            var updatedLoan = new Loan
                              {
                                  Id           = loan.Id,
                                  TypeId       = loan.TypeId,
                                  AccountId    = loan.AccountId,
                                  Amount       = loan.Amount,
                                  Period       = loan.Period,
                                  CreationDate = loan.CreationDate,
                                  MaturityDate = loan.MaturityDate,
                                  CurrencyId   = loan.CurrencyId,
                                  Status       = LoanStatus.Closed,
                                  InterestType = loan.InterestType,
                                  CreatedAt    = loan.CreatedAt,
                                  ModifiedAt   = DateTime.UtcNow
                              };

            await loanRepository.Update(loan, updatedLoan);
            _logger.LogInformation($"Loan {loan.Id} has been marked as completed");
        }
    }

    private async Task<decimal> CalculateInstallmentAmount(Loan loan, Installment installment, IInstallmentRepository installmentRepository)
    {
        
        // Get total monthly payment amount using amortization formula
        decimal monthlyPayment = CalculateMonthlyPayment(loan.Amount, loan.Period, GetEffectiveInterestRate(loan, installment));

        return monthlyPayment;
    }

    private decimal GetEffectiveInterestRate(Loan loan, Installment installment)
    {
        decimal effectiveRate = installment.InterestRate;

        // For variable rate loans, add bank margin and potentially EURIBOR
        if (loan.InterestType == InterestType.Variable)
        {
            // Add bank margin from loan type
            effectiveRate += loan.LoanType.Margin;

            // Add EURIBOR if the loan is in foreign currency
            if (loan.Currency.Code != "RSD")
            {
                effectiveRate += GetCurrentEuriborRate();
            }
        }

        return effectiveRate;
    }

    /*private async Task<decimal> GetRemainingPrincipal(Loan loan, Installment currentInstallment, IInstallmentRepository installmentRepository)
        {
            int paidInstallments = await installmentRepository.GetPaidInstallmentsCountBeforeDateAsync(loan.Id, currentInstallment.ExpectedDueDate);

            // Calculate remaining principal
            decimal principalPaidSoFar = (loan.Amount / loan.Period) * paidInstallments;
            return loan.Amount - principalPaidSoFar;
        }*/
    
    private async Task<decimal> GetRemainingPrincipal(Loan loan, Installment currentInstallment, IInstallmentRepository installmentRepository)
    {
        int paidInstallments = await installmentRepository.GetPaidInstallmentsCountBeforeDateAsync(loan.Id, currentInstallment.ExpectedDueDate);
    
        if (paidInstallments == 0)
            return loan.Amount; // No payments made yet
    
        // Starting values
        decimal remainingPrincipal = loan.Amount;
    
        // For each paid installment, calculate how much principal was paid
        for (int i = 0; i < paidInstallments; i++)
        {
            // Get the effective interest rate for this payment period
            // This is simplified - ideally we would get the actual rate from each past installment
            decimal effectiveRate = currentInstallment.InterestRate;
            if (loan.InterestType == InterestType.Variable)
            {
                effectiveRate += loan.LoanType.Margin;
                if (loan.Currency.Code != "RSD")
                {
                    // For simulation purposes, we're using a fixed rate
                    // In reality, this would vary for each past payment
                    effectiveRate += 3.0m; // Simplified EURIBOR estimate
                }
            }
        
            // Calculate monthly interest rate
            decimal monthlyRate = effectiveRate / 1200;
        
            // Calculate interest for this period
            decimal interestPayment = remainingPrincipal * monthlyRate;
        
            // Calculate total payment for this period
            decimal monthlyPayment = CalculateMonthlyPayment(loan.Amount, loan.Period, effectiveRate);
        
            // Calculate principal payment (payment minus interest)
            decimal principalPayment = monthlyPayment - interestPayment;
        
            // Reduce remaining principal
            remainingPrincipal -= principalPayment;
        }
    
        return remainingPrincipal;
    }

    private async Task<(decimal interest, decimal principal)> CalculatePaymentComponents(Loan                   loan, Installment installment, decimal paymentAmount,
                                                                                         IInstallmentRepository installmentRepository)
    {
        decimal remainingPrincipal = await GetRemainingPrincipal(loan, installment, installmentRepository);
        decimal effectiveRate      = GetEffectiveInterestRate(loan, installment);

        // Monthly interest rate (as decimal, not percentage)
        decimal monthlyRate = effectiveRate / 1200;

        // Calculate interest component
        decimal interestAmount = remainingPrincipal * monthlyRate;

        // Calculate principal component
        decimal principalAmount = paymentAmount - interestAmount;

        return (interestAmount, principalAmount);
    }

    private decimal GetBaseInterestRate(decimal amountInRSD)
    {
        // Base interest rate table based on loan amount ranges
        if (amountInRSD <= 500000) return 6.25m;
        if (amountInRSD <= 1000000) return 6.00m;
        if (amountInRSD <= 2000000) return 5.75m;
        if (amountInRSD <= 5000000) return 5.50m;
        if (amountInRSD <= 10000000) return 5.25m;
        if (amountInRSD <= 20000000) return 5.00m;
        return 4.75m; // For amounts over 20,000,000
    }

    private decimal GetCurrentEuriborRate()
    {
        // Simulating EURIBOR rate between 2.5% and 4.5%
        return (decimal)(2.5 + _random.NextDouble() * 2.0);
    }

    public decimal CalculateMonthlyPayment(decimal loanAmount, int loanPeriodMonths, decimal annualInterestRate)
    {
        // Convert annual rate to monthly (percentage to decimal)
        decimal monthlyRate = annualInterestRate / 1200; // Divide by 100 for percentage to decimal, then by 12 for monthly

        // Calculate monthly payment using the loan amortization formula
        if (monthlyRate == 0)
        {
            return loanAmount / loanPeriodMonths;
        }
        else
        {
            double pow = Math.Pow(1 + (double)monthlyRate, loanPeriodMonths);
            return loanAmount * monthlyRate * (decimal)pow / ((decimal)pow - 1);
        }
    }
    
}
