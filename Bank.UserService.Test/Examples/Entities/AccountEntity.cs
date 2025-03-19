using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Account
        {
            public static readonly AccountCreateRequest CreateRequest = new()
                                                                        {
                                                                            Name          = "Štedni račun",
                                                                            DailyLimit    = 2000.00m,
                                                                            MonthlyLimit  = 50000.00m,
                                                                            ClientId      = Guid.Parse("36bfba9b-2810-4957-8cf5-c9cc40adb7d6"),
                                                                            Balance       = 5000.75m,
                                                                            CurrencyId    = Guid.Parse("6842a5fa-eee4-4438-bcff-5217b6ac6ace"),
                                                                            AccountTypeId = Guid.Parse("f606cd71-f42f-4ca4-a532-5254bfe34920"),
                                                                            Status        = true
                                                                        };

            public static readonly AccountUpdateClientRequest UpdateClientRequest = new()
                                                                                    {
                                                                                        Name         = "Štedni račun",
                                                                                        DailyLimit   = 2000.00M,
                                                                                        MonthlyLimit = 50000.00M
                                                                                    };

            public static readonly AccountUpdateEmployeeRequest UpdateEmployeeRequest = new()
                                                                                        {
                                                                                            Status = false
                                                                                        };
        }
    }
}
