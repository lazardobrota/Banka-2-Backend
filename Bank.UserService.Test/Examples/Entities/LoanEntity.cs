using Bank.Application.Domain;
using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Loan
        {
            public static readonly LoanRequest Request = new()
                                                         {
                                                             TypeId       = Guid.Parse("4632f907-4f31-47f2-8fab-5c8a717aef55"),
                                                             AccountId    = Guid.Parse("b5f4b482-3e93-482f-9687-4d58c473fc4d"),
                                                             Amount       = 50000.00m,
                                                             Period       = 60,
                                                             CurrencyId   = Guid.Parse("88bfe7f0-8f74-42f7-b6ba-07b3145da989"),
                                                             InterestType = InterestType.Mixed
                                                         };

            public static readonly LoanUpdateRequest UpdateRequest = new()
                                                                     {
                                                                         Status       = LoanStatus.Closed,
                                                                         MaturityDate = new(2029, 3, 5)
                                                                     };
        }
    }
}
