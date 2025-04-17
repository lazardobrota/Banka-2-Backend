using Bank.Application.Requests;
using Bank.UserService.Database.Seeders;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class AccountCurrency
    {
        public static readonly AccountCurrencyCreateRequest CreateRequest = new()
                                                                            {
                                                                                EmployeeId   = Seeder.Employee.Employee01.Id,
                                                                                CurrencyId   = Seeder.Currency.SerbianDinar.Id,
                                                                                AccountId    = Seeder.Account.DomesticAccount01.Id,
                                                                                DailyLimit   = 1000,
                                                                                MonthlyLimit = 3000
                                                                            };

        public static readonly AccountCurrencyClientUpdateRequest ClientUpdateRequest = new()
                                                                                        {
                                                                                            DailyLimit   = 2000,
                                                                                            MonthlyLimit = 4000
                                                                                        };
    }
}
