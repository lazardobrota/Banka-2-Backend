using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class LoanType
    {
        public static readonly LoanTypeCreateRequest CreateRequest = new()
                                                                     {
                                                                         Name   = Constant.LoanTypeName,
                                                                         Margin = Constant.LoanTypeMargin,
                                                                     };

        public static readonly LoanTypeUpdateRequest UpdateRequest = new()
                                                                     {
                                                                         Name   = Constant.LoanTypeName,
                                                                         Margin = Constant.LoanTypeMargin,
                                                                     };

        public static readonly LoanTypeResponse Response = new()
                                                           {
                                                               Id     = Constant.Id,
                                                               Name   = Constant.LoanTypeName,
                                                               Margin = Constant.LoanTypeMargin
                                                           };
    }
}
