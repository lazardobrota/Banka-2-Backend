using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.BackgroundServices;
using Bank.UserService.Mappers;
using Bank.UserService.Models;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface ITransactionService
{
    Task<Result<Page<TransactionResponse>>> GetAll(TransactionFilterQuery transactionFilterQuery, Pageable pageable);

    Task<Result<Page<TransactionResponse>>> GetAllByAccountId(Guid accountId, TransactionFilterQuery transactionFilterQuery, Pageable pageable);

    Task<Result<TransactionResponse>> GetOne(Guid id);

    Task<Result<TransactionCreateResponse>> Create(TransactionCreateRequest transactionCreateRequest);

    Task<Result<TransactionResponse>> Update(TransactionUpdateRequest transactionUpdateRequest, Guid id);

    Task ProcessInternalTransactionsForAccount(Guid accountId, List<ProcessTransaction> transactions);

    Task<Result<Transaction>> PrepareInternalTransaction(PrepareInternalTransaction internalTransaction);
}

public class TransactionService(
    ITransactionRepository       transactionRepository,
    ITransactionCodeRepository   transactionCodeRepository,
    IAuthorizationService        authorizationService,
    IAccountRepository           accountRepository,
    ICurrencyRepository          currencyRepository,
    IExchangeRepository          exchangeRepository,
    TransactionBackgroundService transactionBackgroundService
) : ITransactionService
{
    private readonly ITransactionRepository       m_TransactionRepository        = transactionRepository;
    private readonly ITransactionCodeRepository   m_TransactionCodeRepository    = transactionCodeRepository;
    private readonly IAccountRepository           m_AccountRepository            = accountRepository;
    private readonly ICurrencyRepository          m_CurrencyRepository           = currencyRepository;
    private readonly IExchangeRepository          m_ExchangeRepository           = exchangeRepository;
    private readonly IAuthorizationService        m_AuthorizationService         = authorizationService;
    private readonly TransactionBackgroundService m_TransactionBackgroundService = transactionBackgroundService;

    public async Task<Result<Page<TransactionResponse>>> GetAll(TransactionFilterQuery transactionFilterQuery, Pageable pageable)
    {
        var page = await m_TransactionRepository.FindAll(transactionFilterQuery, pageable);

        var transactionResponses = page.Items.Select(transaction => transaction.ToResponse())
                                       .ToList();

        return Result.Ok(new Page<TransactionResponse>(transactionResponses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<TransactionResponse>> GetOne(Guid id)
    {
        var transaction = await m_TransactionRepository.FindById(id);

        if (transaction is null)
            return Result.NotFound<TransactionResponse>($"No Transaction found with Id: {id}");

        if (m_AuthorizationService.Role     == Role.Client && transaction.FromAccount?.ClientId != m_AuthorizationService.UserId &&
            transaction.ToAccount?.ClientId != m_AuthorizationService.UserId)
            return Result.Unauthorized<TransactionResponse>();

        return Result.Ok(transaction.ToResponse());
    }

    public async Task<Result<Page<TransactionResponse>>> GetAllByAccountId(Guid accountId, TransactionFilterQuery transactionFilterQuery, Pageable pageable)
    {
        var page = await m_TransactionRepository.FindAllByAccountId(accountId, transactionFilterQuery, pageable);

        if (m_AuthorizationService.Role == Role.Client &&
            page.Items.Any(transaction => transaction.FromAccount!.ClientId != m_AuthorizationService.UserId && transaction.ToAccount!.ClientId != m_AuthorizationService.UserId))
            return Result.Forbidden<Page<TransactionResponse>>();

        var transactionResponses = page.Items.Select(transaction => transaction.ToResponse())
                                       .ToList();

        return Result.Ok(new Page<TransactionResponse>(transactionResponses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<TransactionCreateResponse>> Create(TransactionCreateRequest transactionCreateRequest)
    {
        var code        = await m_TransactionCodeRepository.FindById(transactionCreateRequest.CodeId);
        var fromAccount = await m_AccountRepository.FindById(transactionCreateRequest.FromAccountId);

        if (code == null || fromAccount == null)
            return Result.BadRequest<TransactionCreateResponse>("Invalid data.");

        var transaction = await CreateTransaction(new TempyTransaction
                                                  {
                                                      FromAccountNumber = fromAccount.AccountNumber,
                                                      FromCurrencyId    = transactionCreateRequest.FromCurrencyId,
                                                      FromAmount        = transactionCreateRequest.Amount,
                                                      ToAccountNumber   = transactionCreateRequest.ToAccountNumber,
                                                      ToCurrencyId      = transactionCreateRequest.ToCurrencyId,
                                                      Purpose           = transactionCreateRequest.Purpose,
                                                      ReferenceNumber   = transactionCreateRequest.ReferenceNumber,
                                                      CodeId            = transactionCreateRequest.CodeId,
                                                  });

        if (transaction.Value == null)
            return Result.BadRequest<TransactionCreateResponse>("Not going thru.");

        return Result.Ok(transaction.Value.ToCreateResponse());
    }

    public async Task<Result<TransactionResponse>> Update(TransactionUpdateRequest transactionUpdateRequest, Guid id)
    {
        var oldTransaction = await m_TransactionRepository.FindById(id);

        if (oldTransaction is null)
            return Result.NotFound<TransactionResponse>($"No Transaction found with Id: {id}");

        var account = await m_TransactionRepository.Update(oldTransaction, transactionUpdateRequest.ToTransaction(oldTransaction));

        return Result.Ok(account.ToResponse());
    }

    public async Task ProcessInternalTransactionsForAccount(Guid accountId, List<ProcessTransaction> transactions)
    {
        var account = await m_AccountRepository.FindById(accountId);

        if (account == null)
            return;

        foreach (var processTransaction in transactions)
        {
            var transaction = await m_TransactionRepository.FindById(processTransaction.TransactionId);

            if (transaction == null || transaction.Status == TransactionStatus.Canceled)
                return;

            var multiplier = processTransaction.FromAccountId == processTransaction.ToAccountId ? 0.5m : 1; // TODO: do it differently

            if (processTransaction.FromAccountId == account.Id)
                account.FindAccountBalance(processTransaction.FromCurrencyId)!.ChangeBalance(-processTransaction.FromAmount * multiplier);

            if (processTransaction.ToAccountId == account.Id)
                account.FindAccountBalance(processTransaction.ToCurrencyId)!.ChangeBalance(processTransaction.ToAmount * multiplier);

            await m_AccountRepository.Update(account, account);

            transaction.Status = transaction.Status switch
                                 {
                                     TransactionStatus.Pending => TransactionStatus.Affirm,
                                     TransactionStatus.Affirm  => TransactionStatus.Completed,
                                     _                         => transaction.Status
                                 };

            await m_TransactionRepository.Update(transaction, transaction);
        }
    }

    public async Task<Result<Transaction>> CreateTransaction(TempyTransaction tempyTransaction)
    {
        if (tempyTransaction.FromAccountNumber == null && tempyTransaction.ToAccountNumber == null)
            return Result.BadRequest<Transaction>("No valid account provided.");

        var bankCodeFrom = tempyTransaction.FromAccountNumber?.Substring(0, 3);
        var bankCodeTo   = tempyTransaction.ToAccountNumber?.Substring(0, 3);

        var fromAccount = await m_AccountRepository.FindByNumber(tempyTransaction.FromAccountNumber?.Substring(7, 9) ?? "");
        var toAccount   = await m_AccountRepository.FindByNumber(tempyTransaction.ToAccountNumber?.Substring(7, 9)   ?? "");

        var fromCurrency = await m_CurrencyRepository.FindById(tempyTransaction.FromCurrencyId);
        var toCurrency   = await m_CurrencyRepository.FindById(tempyTransaction.ToCurrencyId);

        if (bankCodeTo == null && bankCodeFrom == m_TransactionBackgroundService.Bank.Code)
            return await PrepareWithdrawTransaction(new PrepareWithdrawTransaction
                                                    {
                                                        Account  = fromAccount,
                                                        Currency = fromCurrency,
                                                        Amount   = tempyTransaction.FromAmount
                                                    });

        if (bankCodeFrom == null && bankCodeTo == m_TransactionBackgroundService.Bank.Code)
            return await PrepareDepositTransaction(new PrepareDepositTransaction
                                                   {
                                                       Account  = toAccount,
                                                       Currency = toCurrency,
                                                       Amount   = tempyTransaction.ToAmount
                                                   });

        if (bankCodeFrom == null || bankCodeTo == null)
            return Result.BadRequest<Transaction>("Some error");

        var transactionCode = await m_TransactionCodeRepository.FindById(tempyTransaction.CodeId);
        var fromExchange    = await m_ExchangeRepository.FindByCurrencyFromAndCurrencyTo(m_TransactionBackgroundService.DefaultCurrency.Id, tempyTransaction.FromCurrencyId);
        var toExchange      = await m_ExchangeRepository.FindByCurrencyFromAndCurrencyTo(m_TransactionBackgroundService.DefaultCurrency.Id, tempyTransaction.ToCurrencyId);

        if (bankCodeFrom == m_TransactionBackgroundService.Bank.Code && bankCodeTo == m_TransactionBackgroundService.Bank.Code)
            return await PrepareInternalTransaction(new PrepareInternalTransaction
                                                    {
                                                        FromAccount     = fromAccount,
                                                        FromCurrency    = fromCurrency,
                                                        FromAmount      = tempyTransaction.FromAmount,
                                                        FromExchange    = fromExchange,
                                                        ToAccount       = toAccount,
                                                        ToCurrency      = toCurrency,
                                                        ToAmount        = tempyTransaction.ToAmount,
                                                        ToExchange      = toExchange,
                                                        TransactionCode = transactionCode,
                                                        ReferenceNumber = tempyTransaction.ReferenceNumber,
                                                        Purpose         = tempyTransaction.Purpose
                                                    });

        return await AddExternalTransaction(tempyTransaction);
    }

    private async Task<Result<Transaction>> PrepareDepositTransaction(PrepareDepositTransaction depositTransaction)
    {
        if (depositTransaction.Account == null || depositTransaction.Currency == null || depositTransaction.Amount <= 0)
            return Result.BadRequest<Transaction>("Some error");

        var accountToBalance = depositTransaction.Account.FindAccountBalance(depositTransaction.Currency.Id);

        if (accountToBalance == null)
            return Result.BadRequest<Transaction>("Some error");

        accountToBalance.ChangeAvailableBalance(depositTransaction.Amount);

        m_TransactionBackgroundService.BankAccount!.FindAccountBalance(depositTransaction.Currency.Id)!.ChangeAvailableBalance(depositTransaction.Amount);

        await m_AccountRepository.Update(depositTransaction.Account, depositTransaction.Account);

        var transaction = depositTransaction.ToTransaction();

        var processTransaction = depositTransaction.ToProcessTransaction(transaction.Id);

        await m_TransactionRepository.Add(transaction);

        m_TransactionBackgroundService.InternalTransactions.Enqueue(processTransaction);

        return Result.Ok(transaction);
    }

    private async Task<Result<Transaction>> PrepareWithdrawTransaction(PrepareWithdrawTransaction withdrawTransaction)
    {
        if (withdrawTransaction.Account == null || withdrawTransaction.Currency == null || withdrawTransaction.Amount <= 0)
            return Result.BadRequest<Transaction>("Some error");

        var accountFromBalance = withdrawTransaction.Account.FindAccountBalance(withdrawTransaction.Currency.Id);

        if (accountFromBalance == null)
            return Result.BadRequest<Transaction>("Some error");

        if (accountFromBalance.GetAvailableBalance() < withdrawTransaction.Amount)
            return Result.BadRequest<Transaction>("Not enough resources.");

        accountFromBalance.ChangeAvailableBalance(-withdrawTransaction.Amount);

        m_TransactionBackgroundService.BankAccount.FindAccountBalance(withdrawTransaction.Currency.Id)!.ChangeAvailableBalance(-withdrawTransaction.Amount);

        await m_AccountRepository.Update(withdrawTransaction.Account, withdrawTransaction.Account);

        var transaction = withdrawTransaction.ToTransaction();

        var processTransaction = withdrawTransaction.ToProcessTransaction(transaction.Id);

        await m_TransactionRepository.Add(transaction);

        m_TransactionBackgroundService.InternalTransactions.Enqueue(processTransaction);

        return Result.Ok(transaction);
    }

    public async Task<Result<Transaction>> PrepareInternalTransaction(PrepareInternalTransaction internalTransaction)
    {
        if (internalTransaction.ToAccount    == null || internalTransaction.ToCurrency == null || internalTransaction.FromAccount     == null ||
            internalTransaction.FromCurrency == null || internalTransaction.FromAmount <= 0    || internalTransaction.TransactionCode == null)
            return Result.BadRequest<Transaction>("Some error");

        var accountFromBalance = internalTransaction.FromAccount.FindAccountBalance(internalTransaction.FromCurrency.Id);
        var accountToBalance   = internalTransaction.ToAccount.FindAccountBalance(internalTransaction.ToCurrency.Id);

        if (accountFromBalance == null || accountToBalance == null)
            return Result.BadRequest<Transaction>("Some error");

        if (accountFromBalance.GetAvailableBalance() < internalTransaction.FromAmount)
            return Result.BadRequest<Transaction>("Not enough resources.");

        var exchangeRateFrom = internalTransaction.FromExchange?.AskRate ?? 1;
        var exchangeRateTo   = internalTransaction.ToExchange?.BidRate   ?? 1;
        var exchangeRate     = exchangeRateFrom / exchangeRateTo;

        internalTransaction.ToAmount = exchangeRate * internalTransaction.FromAmount;

        var averageRateFrom = internalTransaction.FromExchange?.InverseRate ?? 1;
        var averageRateTo   = internalTransaction.ToExchange?.InverseRate   ?? 1;
        var averageRate     = averageRateFrom / averageRateTo;

        accountFromBalance.ChangeAvailableBalance(-internalTransaction.FromAmount);
        accountToBalance.ChangeAvailableBalance(internalTransaction.ToAmount);

        m_TransactionBackgroundService.BankAccount.FindAccountBalance(internalTransaction.FromCurrency.Id)!.ChangeAvailableBalance(-averageRate * internalTransaction.FromAmount);
        m_TransactionBackgroundService.BankAccount.FindAccountBalance(internalTransaction.ToCurrency.Id)!.ChangeAvailableBalance(internalTransaction.ToAmount);

        await m_AccountRepository.Update(internalTransaction.FromAccount, internalTransaction.FromAccount);
        await m_AccountRepository.Update(internalTransaction.ToAccount,   internalTransaction.ToAccount);

        var transaction = internalTransaction.ToTransaction();

        var processTransaction = internalTransaction.ToProcessTransaction(transaction.Id);

        await m_TransactionRepository.Add(transaction);

        m_TransactionBackgroundService.InternalTransactions.Enqueue(processTransaction);

        return Result.Ok(transaction);
    }

    private async Task<Result<Transaction>> AddExternalTransaction(TempyTransaction tempyTransaction)
    {
        return Result.Forbidden<Transaction>();
    }
}
