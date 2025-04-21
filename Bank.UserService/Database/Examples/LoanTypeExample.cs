using Bank.Application.Requests;

namespace Bank.UserService.Database.Examples;

public static partial class Example
{
    public static class LoanType
    {
        public static readonly LoanTypeCreateRequest CreateRequest = new()
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
