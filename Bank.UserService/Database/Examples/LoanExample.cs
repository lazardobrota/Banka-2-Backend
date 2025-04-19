using Bank.Application.Domain;
using Bank.Application.Requests;

using Seeder = Bank.UserService.Database.Seeders.Seeder;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class Loan
    {
        public static readonly LoanCreateRequest CreateRequest = new()
                                                           {
                                                               TypeId       = Seeder.LoanType.Personal.Id,
                                                               AccountId    = Seeder.Account.DomesticAccount02.Id,
                                                               Amount       = 50000.00m,
                                                               Period       = 60,
                                                               CurrencyId   = Seeder.Currency.SerbianDinar.Id,
                                                               InterestType = InterestType.Mixed
                                                           };

        public static readonly LoanUpdateRequest UpdateRequest = new()
                                                                 {
                                                                     Status       = LoanStatus.Closed,
                                                                     MaturityDate = new(2029, 3, 5)
                                                                 };
    }
}
