using Bank.Application.Requests;

namespace Bank.UserService.Test.Examples.Entities;

public static partial class Example
{
    public static partial class Entity
    {
        public static class LoanType
        {
            public static readonly LoanTypeRequest Request = new()
                                                             {
                                                                 Name   = "Lični kredit",
                                                                 Margin = 3.5m,
                                                             };

            public static readonly LoanTypeUpdateRequest UpdateRequest = new()
                                                                         {
                                                                             Name   = "Lični kredit",
                                                                             Margin = 4m,
                                                                         };
        }
    }
}
