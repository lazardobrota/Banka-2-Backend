using System.Transactions;

using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Models;
using Bank.UserService.Repositories;
using Bank.UserService.Services;

using Transaction = Bank.UserService.Models.Transaction;
using TransactionStatus = Bank.Application.Domain.TransactionStatus;

namespace Bank.UserService.HostedServices;

public class LoanHostedService
{
    private readonly Random                     m_Random = new();
    private readonly ILoanRepository            m_LoanRepository;
    private readonly IAccountRepository         m_AccountRepository;
    private readonly IInstallmentRepository     m_InstallmentRepository;
    private readonly IEmailService              m_EmailService;
    private readonly ITransactionCodeRepository m_TransactionCodeRepository;
    private readonly IExchangeService           m_ExchangeService;
    private readonly ITransactionService        m_transactionService;
    private          Timer?                     m_Timer;

    public LoanHostedService(ILoanRepository loanRepository, IAccountRepository accountRepository, IInstallmentRepository installmentRepository, IEmailService emailService,
                             ITransactionCodeRepository transactionCodeRepository, IExchangeService exchangeService, ITransactionService transactionService)
    {
        m_LoanRepository            = loanRepository;
        m_AccountRepository         = accountRepository;
        m_InstallmentRepository     = installmentRepository;
        m_EmailService              = emailService;
        m_TransactionCodeRepository = transactionCodeRepository;
        m_ExchangeService           = exchangeService;
        m_transactionService        = transactionService;
    }

    public void OnApplicationStarted()
    {
        Initialize();
    }

    public void OnApplicationStopped()
    {
        m_Timer?.Dispose();
    }

    private void Initialize()
    {
        var midnight          = DateTime.Today.AddDays(1);
        var timeLeftUntilNext = midnight.Subtract(DateTime.UtcNow);

        m_Timer = new Timer(async _ => await ProcessLoanPayments(), null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
    }

    public async Task ProcessLoanPayments()
    {
        try
        {
            Console.WriteLine($"\n=== LOAN PAYMENT PROCESSING START - {DateTime.UtcNow} ===");
            var today = DateTime.UtcNow.Date;
            Console.WriteLine($"Processing date: {today}");

            var activeLoans = await m_LoanRepository.GetLoansWithDueInstallmentsAsync(today);
            Console.WriteLine($"Found {activeLoans.Count} active loans with due installments");

            if (activeLoans.Count == 0)
            {
                Console.WriteLine("No loans found - exiting");
                return;
            }

            foreach (var loan in activeLoans)
            {
                Console.WriteLine($"\nProcessing loan ID: {loan.Id}, Amount: {loan.Amount}");
                await ProcessLoanInstallmentsAsync(loan, today, m_LoanRepository, m_AccountRepository, m_InstallmentRepository);
            }
            
            Console.WriteLine("=== LOAN PAYMENT PROCESSING END ===\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in ProcessLoanPayments: {ex.Message}");
            throw new Exception(ex.Message);
        }
    }

    public async Task ProcessLoanInstallmentsAsync(Loan                   loan, DateTime currentDate, ILoanRepository loanRepository, IAccountRepository accountRepository,
                                                   IInstallmentRepository installmentRepository)
    {
        try
        {
            Console.WriteLine($"Processing installments for loan {loan.Id}");
            var dueInstallments = await installmentRepository.GetDueInstallmentsForLoanAsync(loan.Id, currentDate);
            Console.WriteLine($"Found {dueInstallments.Count} due installments");

            if (dueInstallments.Count == 0)
                return;

            foreach (var installment in dueInstallments)
            {
                Console.WriteLine($"Processing installment {installment.Id}, Due: {installment.ExpectedDueDate}");
                var paymentAmount = await CalculateInstallmentAmount(loan);
                Console.WriteLine($"Calculated payment amount: {paymentAmount}");

                // Process payment
                var paymentSuccessful = await ProcessPaymentAsync(loan, paymentAmount, accountRepository);
                Console.WriteLine($"Payment successful: {paymentSuccessful}");

                if (paymentSuccessful)
                {
                    Console.WriteLine("Updating installment to Paid...");
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
                    Console.WriteLine("Installment updated successfully");

                    var client = await GetClientByLoan(loan, accountRepository);

                    if (client != null)
                    {
                        Console.WriteLine($"Sending email to client: {client.FirstName}");
                        // Izračunaj preostali dug
                        var remainingBalance  = await GetRemainingPrincipal(loan, installmentRepository);
                        var installmentAmount = await CalculateInstallmentAmount(loan);
                        await m_EmailService.Send(EmailType.LoanInstallmentPaid, client, client.FirstName, installmentAmount, loan.Currency.Code, remainingBalance);
                        Console.WriteLine("Email sent successfully");
                    }
                    else
                    {
                        Console.WriteLine("No client found for loan");
                    }

                    await CreateNextInstallmentIfNeededAsync(loan, installmentRepository);
                }
                else
                {
                    Console.WriteLine("Payment failed - installment not updated");
                }
            }

            await CheckLoanCompletionAsync(loan, loanRepository, installmentRepository);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in ProcessLoanInstallmentsAsync: {ex.Message}");
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> ProcessPaymentAsync(Loan loan, decimal paymentAmount, IAccountRepository accountRepository)
    {
        try
        {
            Console.WriteLine($"Starting payment process for loan {loan.Id}, amount: {paymentAmount}");
            var account = await accountRepository.FindById(loan.AccountId);

            if (account == null)
            {
                Console.WriteLine($"Account not found for loan {loan.Id}");
                return false;
            }

            Console.WriteLine($"Found account: {account.Number}, Balance: {account.AvailableBalance}");

            var allCodes = await m_TransactionCodeRepository.FindAll(new TransactionCodeFilterQuery(), new Pageable(1, 100));
            Console.WriteLine($"Found {allCodes.TotalElements} transaction codes");
            
            var loanPaymentCode = allCodes.Items.FirstOrDefault(c => c.Code == "289");

            if (loanPaymentCode == null)
            {
                Console.WriteLine("Transaction code '289' not found!");
                Console.WriteLine("Available codes:");
                foreach (var code in allCodes.Items.Take(10))
                {
                    Console.WriteLine($"  Code: {code.Code}, Description: {code.Name}");
                }
                return false;
            }

            Console.WriteLine($"Found transaction code '289': {loanPaymentCode.Name}");

            // Create a transaction request for loan payment
            var transactionRequest = new TransactionCreateRequest
                                     {
                                         FromAccountNumber = account.AccountNumber,
                                         FromCurrencyId    = loan.CurrencyId,
                                         ToAccountNumber   = "000000000000",
                                         ToCurrencyId      = loan.CurrencyId,
                                         Amount            = paymentAmount,
                                         CodeId            = loanPaymentCode.Id,
                                         Purpose           = "loan payment"
                                     };

            Console.WriteLine("Creating transaction...");
            var result = await m_transactionService.Create(transactionRequest);

            if (result.Value != null)
            {
                Console.WriteLine($"Transaction created successfully! ID: {result.Value.Id}");
                return true;
            }
            else
            {
                Console.WriteLine("Transaction creation failed - result.Value is null");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in ProcessPaymentAsync: {ex.Message}");
            return false;
        }
    }

    public async Task CreateNextInstallmentIfNeededAsync(Loan loan, IInstallmentRepository installmentRepository)
    {
        Console.WriteLine($"Checking if next installment needed for loan {loan.Id}");
        var totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);
        Console.WriteLine($"Total installments: {totalInstallments}, Loan period: {loan.Period}");

        if (totalInstallments < loan.Period)
        {
            Console.WriteLine("Creating next installment...");
            var latestInstallment = await installmentRepository.GetLatestInstallmentForLoanAsync(loan.Id);

            var newInstallment = new Installment
                                 {
                                     Id              = Guid.NewGuid(),
                                     LoanId          = loan.Id,
                                     InterestRate    = await GetEffectiveInterestRate(loan),
                                     ExpectedDueDate = latestInstallment!.ExpectedDueDate.AddMonths(1),
                                     Status          = InstallmentStatus.Pending,
                                     CreatedAt       = DateTime.UtcNow,
                                     ModifiedAt      = DateTime.UtcNow
                                 };

            await installmentRepository.Add(newInstallment);
            Console.WriteLine($"Created new installment {newInstallment.Id} due on {newInstallment.ExpectedDueDate}");
        }
        else
        {
            Console.WriteLine("All installments already created");
        }
    }

    public async Task CheckLoanCompletionAsync(Loan loan, ILoanRepository loanRepository, IInstallmentRepository installmentRepository)
    {
        Console.WriteLine($"Checking loan completion for {loan.Id}");
        var allPaid           = await installmentRepository.AreAllInstallmentsPaidAsync(loan.Id);
        var totalInstallments = await installmentRepository.GetInstallmentCountForLoanAsync(loan.Id);

        Console.WriteLine($"All paid: {allPaid}, Total installments: {totalInstallments}");

        if (allPaid && totalInstallments >= loan.Period)
        {
            Console.WriteLine("Loan completed! Closing loan...");
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

            await loanRepository.Update(updatedLoan);
            Console.WriteLine("Loan status updated to Closed");
        }
        else
        {
            Console.WriteLine("Loan not yet completed");
        }
    }

    public async Task<decimal> CalculateInstallmentAmount(Loan loan)
    {
        var loanAmountInRsd = await ConvertToRsd(loan.Amount, loan.Currency!);

        var monthlyPayment = CalculateMonthlyPayment(loanAmountInRsd, loan.Period, await GetEffectiveInterestRate(loan));

        return monthlyPayment;
    }

    public async Task<decimal> GetEffectiveInterestRate(Loan loan)
    {
        var amountInRsd   = await ConvertToRsd(loan.Amount, loan.Currency!);
        var effectiveRate = GetBaseInterestRate(amountInRsd);

        if (loan.InterestType == InterestType.Variable)
        {
            effectiveRate += loan.LoanType!.Margin;

            effectiveRate += GetCurrentEuriborRate();

            effectiveRate /= 12;
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

        var loanAmountInRsd    = await ConvertToRsd(loan.Amount, loan.Currency);
        var remainingPrincipal = loanAmountInRsd;

        var baseInterestRate = GetBaseInterestRate(loanAmountInRsd);

        for (var i = 0; i < paidInstallments; i++)
        {
            var effectiveRate = baseInterestRate;

            if (loan.InterestType == InterestType.Variable)
            {
                effectiveRate += loan.LoanType!.Margin;

                if (loan.Currency.Code != "RSD")
                    effectiveRate += GetCurrentEuriborRate();
            }

            var monthlyRate = effectiveRate / 1200;

            var interestPayment = remainingPrincipal * monthlyRate;

            var monthlyPayment = CalculateMonthlyPayment(loanAmountInRsd, loan.Period, effectiveRate);

            var principalPayment = monthlyPayment - interestPayment;

            remainingPrincipal -= principalPayment;
        }

        return remainingPrincipal;
    }

    public decimal GetBaseInterestRate(decimal amountInRsd)
    {
        if (amountInRsd <= 500000) return 6.25m;
        if (amountInRsd <= 1000000) return 6.00m;
        if (amountInRsd <= 2000000) return 5.75m;
        if (amountInRsd <= 5000000) return 5.50m;
        if (amountInRsd <= 10000000) return 5.25m;
        if (amountInRsd <= 20000000) return 5.00m;
        return 4.75m;
    }

    public decimal GetCurrentEuriborRate()
    {
        return (decimal)(m_Random.NextDouble() * 3.0 - 1.5);
    }

    public decimal CalculateMonthlyPayment(decimal loanAmount, int loanPeriodMonths, decimal annualInterestRate)
    {
        var monthlyRate = annualInterestRate / 1200;

        if (monthlyRate == 0)
            return loanAmount / loanPeriodMonths;

        var pow = Math.Pow(1 + (double)monthlyRate, loanPeriodMonths);
        return loanAmount * monthlyRate * (decimal)pow / ((decimal)pow - 1);
    }

    public async Task<decimal> ConvertToRsd(decimal amount, Currency currency)
    {
        if (currency.Code == "RSD")
            return amount;

        var exchangeBetweenQuery = new ExchangeBetweenQuery
                                   {
                                       CurrencyFromCode = currency.Code,
                                       CurrencyToCode   = "RSD"
                                   };

        var result = await m_ExchangeService.GetByCurrencies(exchangeBetweenQuery);

        var convertedAmount = amount * result.Value!.Rate;

        return convertedAmount;
    }

    private async Task<User?> GetClientByLoan(Loan loan, IAccountRepository accountRepository)
    {
        var account = await accountRepository.FindById(loan.AccountId);
        return account?.Client;
    }

    public async Task<bool> DisperseFundsAfterLoanActivation(Loan loan)
    {
        try
        {
            // Get the client's account
            var account = await m_AccountRepository.FindById(loan.AccountId);

            if (account == null)
            {
                return false;
            }

            // Create a transaction request for loan disbursement
            var transactionRequest = new TransactionCreateRequest
                                     {
                                         FromAccountNumber = "000000000000", // Bank is the source, can be empty for deposits
                                         FromCurrencyId    = loan.CurrencyId,
                                         ToAccountNumber   = account.AccountNumber,
                                         ToCurrencyId      = loan.CurrencyId,
                                         Amount            = loan.Amount,
                                         CodeId            = new Guid("38259d40-8fc1-4f3d-bc4d-02b8a0283400"), // Loan disbursement code
                                         ReferenceNumber   = "12345",
                                         Purpose           = "Loan disbursement"
                                     };

            // Use TransactionService to create the transaction
            await m_transactionService.Create(transactionRequest);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}