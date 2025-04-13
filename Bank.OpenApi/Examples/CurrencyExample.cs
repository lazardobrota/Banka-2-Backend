using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Currency
    {
        public static readonly CurrencyResponse Response = new()
                                                           {
                                                               Id          = Constant.Id,
                                                               Name        = Constant.CurrencyName,
                                                               Code        = Constant.CurrencyCode,
                                                               Symbol      = Constant.CurrencySymbol,
                                                               Countries   = [Country.SimpleResponse],
                                                               Description = Constant.Description,
                                                               Status      = Constant.Boolean,
                                                               CreatedAt   = Constant.CreatedAt,
                                                               ModifiedAt  = Constant.ModifiedAt,
                                                           };

        public static readonly CurrencySimpleResponse SimpleResponse = new()
                                                                       {
                                                                           Id          = Constant.Id,
                                                                           Name        = Constant.CurrencyName,
                                                                           Code        = Constant.CurrencyCode,
                                                                           Symbol      = Constant.CurrencySymbol,
                                                                           Description = Constant.Description,
                                                                           Status      = Constant.Boolean,
                                                                           CreatedAt   = Constant.CreatedAt,
                                                                           ModifiedAt  = Constant.ModifiedAt,
                                                                       };
    }
}
