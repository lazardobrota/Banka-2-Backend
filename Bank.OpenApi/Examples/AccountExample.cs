using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Account
    {
        public static readonly AccountCreateRequest CreateRequest = new()
                                                                    {
                                                                        Name          = Constant.AccountName,
                                                                        DailyLimit    = Constant.DailyLimit,
                                                                        MonthlyLimit  = Constant.MonthlyLimit,
                                                                        ClientId      = Constant.Id,
                                                                        Balance       = Constant.Balance,
                                                                        CurrencyId    = Constant.Id,
                                                                        AccountTypeId = Constant.Id,
                                                                        Status        = Constant.Boolean
                                                                    };

        public static readonly AccountUpdateClientRequest UpdateClientRequest = new()
                                                                                {
                                                                                    Name         = Constant.AccountName,
                                                                                    DailyLimit   = Constant.DailyLimit,
                                                                                    MonthlyLimit = Constant.MonthlyLimit,
                                                                                };

        public static readonly AccountUpdateEmployeeRequest UpdateEmployeeRequest = new()
                                                                                    {
                                                                                        Status = Constant.Boolean
                                                                                    };

        public static readonly AccountResponse Response = new()
                                                          {
                                                              Id                = Constant.Id,
                                                              AccountNumber     = Constant.AccountNumber,
                                                              Name              = Constant.AccountName,
                                                              Balance           = Constant.Balance,
                                                              AvailableBalance  = Constant.Balance,
                                                              Type              = AccountType.Response,
                                                              Currency          = Currency.Response,
                                                              Employee          = Employee.SimpleResponse,
                                                              Client            = Client.SimpleResponse,
                                                              AccountCurrencies = [AccountCurrency.Response],
                                                              DailyLimit        = Constant.DailyLimit,
                                                              MonthlyLimit      = Constant.MonthlyLimit,
                                                              CreationDate      = Constant.CreationDate,
                                                              ExpirationDate    = Constant.CreationDate,
                                                              Status            = Constant.Boolean,
                                                              CreatedAt         = Constant.CreatedAt,
                                                              ModifiedAt        = Constant.ModifiedAt
                                                          };

        public static readonly AccountSimpleResponse SimpleResponse = new()
                                                                      {
                                                                          Id            = Constant.Id,
                                                                          AccountNumber = Constant.AccountNumber,
                                                                      };
    }
}
