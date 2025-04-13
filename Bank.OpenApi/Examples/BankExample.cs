using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public class BankExample
    {
        public static readonly BankResponse Response = new()
                                                       {
                                                           Id         = Constant.Id,
                                                           Name       = Constant.BankName,
                                                           Code       = Constant.BankCode,
                                                           BaseUrl    = Constant.BankBaseUrl,
                                                           CreatedAt  = Constant.CreatedAt,
                                                           ModifiedAt = Constant.ModifiedAt,
                                                       };
    }
}
