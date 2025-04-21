using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Bank.Application.Domain;
using Bank.Application.Responses;
using Bank.Application.Utilities;
using Bank.UserService.Configurations;
using Bank.UserService.Database.Seeders;
using Bank.UserService.Mappers;
using Bank.UserService.Models;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

using BankModel = Models.Bank;

public interface IDataService
{
    public BankModel Bank            { get; }
    public Currency  DefaultCurrency { get; }
    public Account   BankAccount     { get; }

    Task<(ImmutableArray<AccountCurrency> values, bool upToDate)> GetAccountCurrencies();

    Task<(ImmutableArray<Account> values, bool upToDate)> GetAccounts();

    Task<(ImmutableArray<AccountType> values, bool upToDate)> GetAccountTypes();

    Task<(ImmutableArray<BankModel> values, bool upToDate)> GetBanks();

    Task<(ImmutableArray<Card> values, bool upToDate)> GetCards();

    Task<(ImmutableArray<CardType> values, bool upToDate)> GetCardTypes();

    Task<(ImmutableArray<Company> values, bool upToDate)> GetCompanies();

    Task<(ImmutableArray<Country> values, bool upToDate)> GetCountries();

    Task<(ImmutableArray<Currency> values, bool upToDate)> GetCurrencies();

    Task<(ImmutableArray<Exchange> values, bool upToDate)> GetCurrencyExchanges();

    Task<(ImmutableArray<Installment> values, bool upToDate)> GetInstallments();

    Task<(ImmutableArray<Loan> values, bool upToDate)> GetLoans();

    Task<(ImmutableArray<LoanType> values, bool upToDate)> GetLoanTypes();

    Task<(ImmutableArray<TransactionCode> values, bool upToDate)> GetTransactionCodes();

    Task<(ImmutableArray<Transaction> values, bool upToDate)> GetTransactions();

    Task<(ImmutableArray<TransactionTemplate> values, bool upToDate)> GetTransactionTemplates();

    Task<(ImmutableArray<User> values, bool upToDate)> GetUsers();
}

[SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
public class DataService : IDataService
{
    private readonly IAccountCurrencyRepository     m_AccountCurrencyRepository;
    private readonly IAccountRepository             m_AccountRepository;
    private readonly IAccountTypeRepository         m_AccountTypeRepository;
    private readonly IBankRepository                m_BankRepository;
    private readonly ICardRepository                m_CardRepository;
    private readonly ICardTypeRepository            m_CardTypeRepository;
    private readonly ICompanyRepository             m_CompanyRepository;
    private readonly ICountryRepository             m_CountryRepository;
    private readonly ICurrencyRepository            m_CurrencyRepository;
    private readonly IExchangeRepository            m_ExchangeRepository;
    private readonly IInstallmentRepository         m_InstallmentRepository;
    private readonly ILoanRepository                m_LoanRepository;
    private readonly ILoanTypeRepository            m_LoanTypeRepository;
    private readonly ITransactionCodeRepository     m_TransactionCodeRepository;
    private readonly ITransactionRepository         m_TransactionRepository;
    private readonly ITransactionTemplateRepository m_TransactionTemplateRepository;
    private readonly IUserRepository                m_UserRepository;
    private readonly IHttpClientFactory             m_HttpClientFactory;

    public BankModel Bank            { get; }
    public Currency  DefaultCurrency { get; }
    public Account   BankAccount     { get; }

    public DataService(IAccountCurrencyRepository     accountCurrencyRepository, IAccountRepository accountRepository, IAccountTypeRepository accountTypeRepository,
                       IBankRepository                bankRepository, ICardRepository cardRepository, ICardTypeRepository cardTypeRepository, ICompanyRepository companyRepository,
                       ICountryRepository             countryRepository, ICurrencyRepository currencyRepository, IExchangeRepository exchangeRepository,
                       IInstallmentRepository         installmentRepository, ILoanRepository loanRepository, ILoanTypeRepository loanTypeRepository,
                       ITransactionCodeRepository     transactionCodeRepository, ITransactionRepository transactionRepository,
                       ITransactionTemplateRepository transactionTemplateRepository, IUserRepository userRepository, IHttpClientFactory httpClientFactory)
    {
        m_AccountCurrencyRepository     = accountCurrencyRepository;
        m_AccountRepository             = accountRepository;
        m_AccountTypeRepository         = accountTypeRepository;
        m_BankRepository                = bankRepository;
        m_CardRepository                = cardRepository;
        m_CardTypeRepository            = cardTypeRepository;
        m_CompanyRepository             = companyRepository;
        m_CountryRepository             = countryRepository;
        m_CurrencyRepository            = currencyRepository;
        m_ExchangeRepository            = exchangeRepository;
        m_InstallmentRepository         = installmentRepository;
        m_LoanRepository                = loanRepository;
        m_LoanTypeRepository            = loanTypeRepository;
        m_TransactionCodeRepository     = transactionCodeRepository;
        m_TransactionRepository         = transactionRepository;
        m_TransactionTemplateRepository = transactionTemplateRepository;
        m_UserRepository                = userRepository;
        m_HttpClientFactory             = httpClientFactory;

        Bank = m_BankRepository.FindById(Seeder.Bank.Bank02.Id)
                               .ConfigureAwait(false)
                               .GetAwaiter()
                               .GetResult() ?? throw new Exception("Invalid bank.");

        DefaultCurrency = m_CurrencyRepository.FindByCode(Configuration.Exchange.DefaultCurrencyCode)
                                              .ConfigureAwait(false)
                                              .GetAwaiter()
                                              .GetResult() ?? throw new Exception("Invalid default currency.");

        BankAccount = m_AccountRepository.FindById(Seeder.Account.BankAccount.Id)
                                         .ConfigureAwait(false)
                                         .GetAwaiter()
                                         .GetResult() ?? throw new Exception("Invalid bank account.");
    }

    public async Task<(ImmutableArray<AccountCurrency> values, bool upToDate)> GetAccountCurrencies()
    {
        return (Seeder.AccountCurrency.All, await m_AccountCurrencyRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Account> values, bool upToDate)> GetAccounts()
    {
        return (Seeder.Account.All, await m_AccountRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<AccountType> values, bool upToDate)> GetAccountTypes()
    {
        return (Seeder.AccountType.All, await m_AccountTypeRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<BankModel> values, bool upToDate)> GetBanks()
    {
        return (Seeder.Bank.All, await m_BankRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Card> values, bool upToDate)> GetCards()
    {
        return (Seeder.Card.All, await m_CardRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<CardType> values, bool upToDate)> GetCardTypes()
    {
        return (Seeder.CardType.All, await m_CardTypeRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Company> values, bool upToDate)> GetCompanies()
    {
        return (Seeder.Company.All, await m_CompanyRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Country> values, bool upToDate)> GetCountries()
    {
        return (Seeder.Country.All, await m_CountryRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Currency> values, bool upToDate)> GetCurrencies()
    {
        return (Seeder.Currency.All, await m_CurrencyRepository.IsEmpty() is not true);
    }

    [SuppressMessage("ReSharper", "UseCollectionExpression")]
    [SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
    public async Task<(ImmutableArray<Exchange> values, bool upToDate)> GetCurrencyExchanges()
    {
        if (Configuration.Application.Profile == Profile.Testing)
            return (Seeder.Exchange.All, false);

        var httpClient = m_HttpClientFactory.CreateClient();

        var exchangeApiResponseMessageTask = httpClient.GetAsync(Configuration.Exchange.ApiUrlTemplate);
        var currenciesTask                 = m_CurrencyRepository.FindAll(new());
        var latestCurrencyExchangesTask    = m_ExchangeRepository.FindAllLatest();

        await Task.WhenAll(exchangeApiResponseMessageTask, currenciesTask);

        var currencies                 = await currenciesTask;
        var latestCurrencyExchanges    = await latestCurrencyExchangesTask;
        var exchangeApiResponseMessage = await exchangeApiResponseMessageTask;
        var exchangeApiResponse        = await exchangeApiResponseMessage.Content.ReadFromJsonAsync<Dictionary<string, ExchangeFetchResponse>>();

        var currencyDictionary = currencies.ToDictionary(currency => currency.Code, currency => currency);
        var defaultCurrency    = currencyDictionary.TryGetValue(Configuration.Exchange.DefaultCurrencyCode, out var value) ? value : null;
        var updatedToday       = latestCurrencyExchanges.Count is not 0 && latestCurrencyExchanges.All(exchange => exchange.CreatedAt.Date == DateTime.Today);

        var latestCurrencyExchangesDictionary = latestCurrencyExchanges.ToDictionary(currencyExchange => currencyExchange.CurrencyTo!.Code,
                                                                                     currencyExchange => currencyExchange.Commission);

        if (!exchangeApiResponseMessage.IsSuccessStatusCode || defaultCurrency is null || exchangeApiResponse is null)
            return ([], updatedToday);

        var currencyExchanges = exchangeApiResponse.Values.Where(exchangeApiEntity => currencyDictionary.ContainsKey(exchangeApiEntity.Code))
                                                   .Select(exchangeApiEntity => exchangeApiEntity.ToExchange(defaultCurrency, currencyDictionary[exchangeApiEntity.Code],
                                                                                                             latestCurrencyExchangesDictionary
                                                                                                             .GetValueOrDefault(exchangeApiEntity.Code, 0.005m)))
                                                   .ToImmutableArray();

        return (currencyExchanges, updatedToday);
    }

    public async Task<(ImmutableArray<Installment> values, bool upToDate)> GetInstallments()
    {
        return (Seeder.Installment.All, await m_InstallmentRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Loan> values, bool upToDate)> GetLoans()
    {
        return (Seeder.Loan.All, await m_LoanRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<LoanType> values, bool upToDate)> GetLoanTypes()
    {
        return (Seeder.LoanType.All, await m_LoanTypeRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<TransactionCode> values, bool upToDate)> GetTransactionCodes()
    {
        return (Seeder.TransactionCode.All, await m_TransactionCodeRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<Transaction> values, bool upToDate)> GetTransactions()
    {
        return (Seeder.Transaction.All, await m_TransactionRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<TransactionTemplate> values, bool upToDate)> GetTransactionTemplates()
    {
        return (Seeder.TransactionTemplate.All, await m_TransactionTemplateRepository.IsEmpty() is not true);
    }

    public async Task<(ImmutableArray<User> values, bool upToDate)> GetUsers()
    {
        var immutableBuilder = ImmutableArray.CreateBuilder<User>();

        immutableBuilder.AddRange(Seeder.Client.All.Select(client =>
                                                           {
                                                               client.Password = HashingUtilities.HashPassword(client.Password!, client.Salt);

                                                               return client.ToUser();
                                                           }));

        immutableBuilder.AddRange(Seeder.Employee.All.Select(employee =>
                                                             {
                                                                 employee.Password = HashingUtilities.HashPassword(employee.Password!, employee.Salt);

                                                                 return employee.ToUser();
                                                             }));

        return (immutableBuilder.ToImmutable(), await m_UserRepository.IsEmpty() is not true);
    }
}
