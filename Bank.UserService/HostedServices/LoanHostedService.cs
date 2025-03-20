using System.Transactions;

using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.UserService.Models;
using Bank.UserService.Repositories;
using Bank.UserService.Services;

using Transaction = Bank.UserService.Models.Transaction;
using TransactionStatus = Bank.Application.Domain.TransactionStatus;

namespace Bank.UserService.HostedServices;

public class LoanHostedService
{
    private readonly ILogger<LoanHostedService> _logger;
    private readonly Random                     _random = new();
    private readonly IServiceProvider           _serviceProvider;
    private          Timer?                     _timer;

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
        _timer = new Timer(async _ => await ProcessLoanPayments(), null, timeLeftUntilNext, TimeSpan.FromMinutes(3));
    }

    public async Task ProcessLoanPayments()
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
                await ProcessLoanInstallmentsAsync(loan, today, loanRepository, accountRepository, installmentRepository);

            _logger.LogInformation("Completed loan payment processing");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during loan payment processing");
        }
    }

    public async Task ProcessLoanInstallmentsAsync(Loan                   loan, DateTime currentDate, ILoanRepository loanRepository, IAccountRepository accountRepository,
                                                   IInstallmentRepository installmentRepository)
    {
        try
        {
            var dueInstallments = await installmentRepository.GetDueInstallmentsForLoanAsync(loan.Id, currentDate);

            if (dueInstallments.Count == 0)
                return;

            _logger.LogInformation($"Processing {dueInstallments.Count} installments for loan {loan.Id}");

            foreach (var installment in dueInstallments)
            {
                var paymentAmount = await CalculateInstallmentAmount(loan, installment, installmentRepository);
                _logger.LogInformation($"Calculated payment amount: {paymentAmount} for installment {installment.Id}");

                // Process payment
                var paymentSuccessful = await ProcessPaymentAsync(loan, installment, paymentAmount, accountRepository);

                if (paymentSuccessful)
                {
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

                    await CreateNextInstallmentIfNeededAsync(loan, installment, loanRepository, installmentRepository);
                }
                else
                {
                    _logger.LogWarning($"Payment processing failed for installment {installment.Id}");
                }
            }

            await CheckLoanCompletionAsync(loan, loanRepository, installmentRepository);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing installments for loan {loan.Id}");
        }
    }

    public async Task<bool> ProcessPaymentAsync(Loan loan, Installment installment, decimal paymentAmount, IAccountRepository accountRepository)
    {
        try
        {
            var account = await accountRepository.FindById(loan.AccountId);

            if (account == null)
            {
                _logger.LogWarning($"Account {loan.AccountId} not found for loan {loan.Id}");
                return false;
            }

            if (account.AvailableBalance < paymentAmount)
            {
                _logger.LogWarning($"Insufficient funds in account {account.Id} for loan payment. Available: {account.AvailableBalance}, Required: {paymentAmount}");
                return false;
            }

            using var scope               = _serviceProvider.CreateScope();
            var       transactionRepo     = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
            var       transactionCodeRepo = scope.ServiceProvider.GetRequiredService<ITransactionCodeRepository>();

            var allCodes        = await transactionCodeRepo.FindAll(new Pageable());
            var loanPaymentCode = allCodes.Items.FirstOrDefault(c => c.Code == "289");

            if (loanPaymentCode == null)
            {
                _logger.LogWarning("Loan payment transaction code not found");
                return false;
            }

            var transaction = new Transaction
                              {
                                  Id              = Guid.NewGuid(),
                                  FromAccountId   = account.Id,
                                  ToAccountId     = null,
                                  FromAmount      = paymentAmount,
                                  ToAmount        = paymentAmount,
                                  FromCurrencyId  = loan.CurrencyId,
                                  ToCurrencyId    = loan.CurrencyId,
                                  CodeId          = loanPaymentCode.Id,
                                  ReferenceNumber = "loan payment",
                                  Purpose         = "loan payment purpose",
                                  Status          = TransactionStatus.Completed,
                                  CreatedAt       = DateTime.UtcNow,
                                  ModifiedAt      = DateTime.UtcNow
                              };

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var updatedAccount = new Account
                                     {
                                         Id                = account.Id,
                                         ClientId          = account.ClientId,
                                         Name              = account.Name,
                                         Number            = account.Number,
                                         Balance           = account.Balance          - paymentAmount,
                                         AvailableBalance  = account.AvailableBalance - paymentAmount,
                                         EmployeeId        = account.EmployeeId,
                                         CurrencyId        = account.CurrencyId,
                                         AccountTypeId     = account.AccountTypeId,
                                         AccountCurrencies = account.AccountCurrencies,
                                         DailyLimit        = account.DailyLimit,
                                         MonthlyLimit      = account.MonthlyLimit,
                                         CreationDate      = account.CreationDate,
                                         ExpirationDate    = account.ExpirationDate,
                                         Status            = account.Status,
                                         CreatedAt         = account.CreatedAt,
                                         ModifiedAt        = DateTime.UtcNow
                                     };

                await accountRepository.Update(account, updatedAccount);

                await transactionRepo.Add(transaction);

                transactionScope.Complete();

                _logger.LogInformation($"Payment of {paymentAmount} {loan.Currency?.Code} processed for loan {loan.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accounts during loan payment processing");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing payment for loan {loan.Id}, installment {installment.Id}");
            return false;
        }
    }

    public async Task CreateNextInstallmentIfNeededAsync(Loan loan, Installment currentInstallment, ILoanRepository loanRepository, IInstallmentRepository installmentRepository)
    {
        var totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);

        if (totalInstallments < loan.Period)
        {
            var latestInstallment = await installmentRepository.GetLatestInstallmentForLoanAsync(loan.Id);

            var newInstallment = new Installment
                                 {
                                     Id              = Guid.NewGuid(),
                                     LoanId          = loan.Id,
                                     InterestRate    = await GetEffectiveInterestRate(loan),
                                     ExpectedDueDate = latestInstallment.ExpectedDueDate.AddMonths(1),
                                     Status          = InstallmentStatus.Pending,
                                     CreatedAt       = DateTime.UtcNow,
                                     ModifiedAt      = DateTime.UtcNow
                                 };

            await installmentRepository.Add(newInstallment);
            _logger.LogInformation($"Created next installment for loan {loan.Id} due on {newInstallment.ExpectedDueDate}");
        }
    }

    public async Task CheckLoanCompletionAsync(Loan loan, ILoanRepository loanRepository, IInstallmentRepository installmentRepository)
    {
        var allPaid           = await installmentRepository.AreAllInstallmentsPaidAsync(loan.Id);
        var totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);

        if (allPaid && totalInstallments >= loan.Period)
        {
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

    public async Task<decimal> CalculateInstallmentAmount(Loan loan, Installment installment, IInstallmentRepository installmentRepository)
    {
        var loanAmountInRSD = await ConvertToRSD(loan.Amount, loan.Currency);
        _logger.LogInformation($"Calculating installment amount for {loanAmountInRSD} AAAAAAAAAAAAAAAAAAAAAAA");

        var monthlyPayment = CalculateMonthlyPayment(loanAmountInRSD, loan.Period, await GetEffectiveInterestRate(loan));

        return monthlyPayment;
    }

    public async Task<decimal> GetEffectiveInterestRate(Loan loan)
    {
        var amountInRSD   = await ConvertToRSD(loan.Amount, loan.Currency);
        var effectiveRate = GetBaseInterestRate(amountInRSD);

        if (loan.InterestType == InterestType.Variable)
        {
            effectiveRate += loan.LoanType.Margin;

            effectiveRate += GetCurrentEuriborRate();
        }

        return effectiveRate;
    }

    public async Task<decimal> GetRemainingPrincipal(Loan loan, IInstallmentRepository installmentRepository)
    {
        var latestInstallment = await installmentRepository.GetLatestInstallmentForLoanAsync(loan.Id);

        if (latestInstallment == null)
            return loan.Amount;

        var paidInstallments = await installmentRepository.GetPaidInstallmentsCountBeforeDateAsync(loan.Id, latestInstallment.ExpectedDueDate);

        if (paidInstallments == 0)
            return loan.Amount;

        var loanAmountInRSD    = await ConvertToRSD(loan.Amount, loan.Currency);
        var remainingPrincipal = loanAmountInRSD;

        var baseInterestRate = GetBaseInterestRate(loanAmountInRSD);

        for (var i = 0; i < paidInstallments; i++)
        {
            var effectiveRate = baseInterestRate;

            if (loan.InterestType == InterestType.Variable)
            {
                effectiveRate += loan.LoanType.Margin;

                if (loan.Currency.Code != "RSD")
                    effectiveRate += GetCurrentEuriborRate();
            }

            var monthlyRate = effectiveRate / 1200;

            var interestPayment = remainingPrincipal * monthlyRate;

            var monthlyPayment = CalculateMonthlyPayment(loanAmountInRSD, loan.Period, effectiveRate);

            var principalPayment = monthlyPayment - interestPayment;

            remainingPrincipal -= principalPayment;
        }

        return remainingPrincipal;
    }

    public async Task<(decimal interest, decimal principal)> CalculatePaymentComponents(Loan                   loan, Installment installment, decimal paymentAmount,
                                                                                        IInstallmentRepository installmentRepository)
    {
        var remainingPrincipal = await GetRemainingPrincipal(loan, installmentRepository);
        var effectiveRate      = await GetEffectiveInterestRate(loan);

        var monthlyRate = effectiveRate / 1200;

        var interestAmount = remainingPrincipal * monthlyRate;

        var principalAmount = paymentAmount - interestAmount;

        return (interestAmount, principalAmount);
    }

    public decimal GetBaseInterestRate(decimal amountInRSD)
    {
        if (amountInRSD <= 500000) return 6.25m;
        if (amountInRSD <= 1000000) return 6.00m;
        if (amountInRSD <= 2000000) return 5.75m;
        if (amountInRSD <= 5000000) return 5.50m;
        if (amountInRSD <= 10000000) return 5.25m;
        if (amountInRSD <= 20000000) return 5.00m;
        return 4.75m;
    }

    public decimal GetCurrentEuriborRate()
    {
        return (decimal)(_random.NextDouble() * 3.0 - 1.5);
    }

    public decimal CalculateMonthlyPayment(decimal loanAmount, int loanPeriodMonths, decimal annualInterestRate)
    {
        var monthlyRate = annualInterestRate / 1200;

        if (monthlyRate == 0)
            return loanAmount / loanPeriodMonths;

        var pow = Math.Pow(1 + (double)monthlyRate, loanPeriodMonths);
        return loanAmount * monthlyRate * (decimal)pow / ((decimal)pow - 1);
    }

    public async Task<decimal> ConvertToRSD(decimal amount, Currency currency)
    {
        if (currency.Code == "RSD")
            return amount;

        using var scope           = _serviceProvider.CreateScope();
        var       exchangeService = scope.ServiceProvider.GetRequiredService<IExchangeService>();

        if (currency.Name == "RSD")
            return amount;

        var exchangeBetweenQuery = new ExchangeBetweenQuery
                                   {
                                       CurrencyFromCode = currency.Code,
                                       CurrencyToCode   = "RSD"
                                   };

        var result = await exchangeService.GetByCurrencies(exchangeBetweenQuery);

        var convertedAmount = amount * result.Value.Rate;

        return convertedAmount;
    }
}
