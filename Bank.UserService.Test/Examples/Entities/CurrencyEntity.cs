using Bank.Application.Queries;
using Bank.UserService.Database.Seeders;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Currency
        {
            public static readonly CurrencyFilterQuery FilterQueryWithName = new()
                                                                         {
                                                                             Name                  = "Euro",
                                                                         };
            public static readonly CurrencyFilterQuery FilterQueryWithCode = new()
                                                                             {
                                                                                 Code                  = "EUR",
                                                                             };

            public static readonly Guid GetById = Seeder.Currency.AustralianDollar.Id;
        }
    }
}
