using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class Card
        {
            public static readonly CardCreateRequest CreateRequest = new()
                                                                     {
                                                                         CardTypeId = Guid.Parse("cd2ea450-14f3-4c46-a35a-7dccf783f48a"),
                                                                         AccountId  = Guid.Parse("5d5fa996-9533-421c-a319-cd43ff41d86f"),
                                                                         Name       = "Credit Card",
                                                                         Limit      = 5000.00m,
                                                                         Status     = true
                                                                     };

            public static readonly CardUpdateStatusRequest UpdateStatusRequest = new()
                                                                                 {
                                                                                     Status = false
                                                                                 };

            public static readonly CardUpdateLimitRequest UpdateLimitRequest = new()
                                                                               {
                                                                                   Limit = 10000.00m
                                                                               };
        }
    }
}
