using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Models;

namespace Bank.UserService.Mappers;

public static class AccountMapper
{
    public static AccountResponse ToResponse(this Account account)
    {
        return new AccountResponse
               {
                   Id            = account.Id,
                   AccountNumber = account.Number,
                   Name          = account.Name,
                   Client = account.Client.ToClient()
                                   .ToSimpleResponse(),
                   Balance          = account.Balance,
                   AvailableBalance = account.AvailableBalance,
                   Employee = account.Employee.ToEmployee()
                                     .ToSimpleResponse(),
                   Currency          = account.Currency.ToResponse(),
                   Type              = account.Type.ToResponse(),
                   AccountCurrencies = MapAccountCurrencies(account.AccountCurrencies),
                   DailyLimit        = 0,
                   MonthlyLimit      = 0,
                   CreationDate      = account.CreationDate,
                   ExpirationDate    = account.ExpirationDate,
                   Status            = account.Status,
                   CreatedAt         = account.CreatedAt,
                   ModifiedAt        = account.ModifiedAt
               };
    }

    private static List<AccountCurrencyResponse> MapAccountCurrencies(List<AccountCurrency> accountCurrencies) =>
    accountCurrencies.Select(accountCurrency => accountCurrency.ToResponse())
                     .ToList();

    public static AccountSimpleResponse ToSimpleResponse(this Account account)
    {
        return new AccountSimpleResponse
               {
                   Id            = account.Id,
                   AccountNumber = account.Number
               };
    }

    public static Account ToAccount(this AccountCreateRequest accountCreateRequest, User employee, User client, Currency currency, AccountType accountType)
    {
        var account = new Account
                      {
                          Id                = Guid.NewGuid(),
                          EmployeeId        = employee.Id,
                          Employee          = employee,
                          CurrencyId        = currency.Id,
                          Currency          = currency,
                          Balance           = accountCreateRequest.Balance,
                          AvailableBalance  = accountCreateRequest.Balance,
                          DailyLimit        = accountCreateRequest.DailyLimit,
                          MonthlyLimit      = accountCreateRequest.MonthlyLimit,
                          CreatedAt         = DateTime.UtcNow,
                          ModifiedAt        = DateTime.UtcNow,
                          Client            = client,
                          ClientId          = client.Id,
                          Name              = accountCreateRequest.Name,
                          Number            = GenerateAccountNumber(),
                          Type              = accountType,
                          AccountTypeId     = accountType.Id,
                          AccountCurrencies = [],
                          CreationDate      = DateOnly.FromDateTime(DateTime.UtcNow),
                          ExpirationDate    = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)),
                          Status            = accountCreateRequest.Status
                      };

        return account;
    }

    private static string GenerateAccountNumber()
    {
        Random random       = new Random();
        string randomDigits = "";

        for (int i = 0; i < 9; i++)
            randomDigits += random.Next(0, 10)
                                  .ToString();

        return randomDigits;
    }
}
