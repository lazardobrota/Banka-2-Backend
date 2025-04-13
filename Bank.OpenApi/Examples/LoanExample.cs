using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Loan
    {
        public static readonly LoanCreateRequest Request = new()
                                                           {
                                                               TypeId       = Constant.Id,
                                                               AccountId    = Constant.Id,
                                                               Amount       = Constant.Amount,
                                                               Period       = Constant.Period,
                                                               CurrencyId   = Constant.Id,
                                                               InterestType = Constant.InterestType,
                                                           };

        public static readonly LoanUpdateRequest UpdateRequest = new()
                                                                 {
                                                                     Status       = Constant.LoanStatus,
                                                                     MaturityDate = Constant.CreatedAt,
                                                                 };

        public static readonly LoanResponse Response = new()
                                                       {
                                                           Id           = Constant.Id,
                                                           Type         = LoanType.Response,
                                                           Account      = Account.Response,
                                                           Amount       = Constant.Amount,
                                                           Period       = Constant.Period,
                                                           CreationDate = Constant.CreationDate,
                                                           MaturityDate = Constant.CreationDate,
                                                           Currency     = Currency.Response,
                                                           Status       = Constant.LoanStatus,
                                                           InterestType = Constant.InterestType,
                                                           CreatedAt    = Constant.CreatedAt,
                                                           ModifiedAt   = Constant.ModifiedAt,
                                                       };
    }
}
