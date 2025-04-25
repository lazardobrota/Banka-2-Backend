using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class StockExchange
    {
        public static readonly ExchangeCreateRequest DefaultCreateRequest = new()
                                                                            {
                                                                                Name       = Constant.CompanyName,
                                                                                Acronym    = Constant.Acronym,
                                                                                MIC        = Constant.MIC,
                                                                                Polity     = Constant.CountryName,
                                                                                TimeZone   = Constant.TimeZone,
                                                                                CurrencyId = Constant.Id
                                                                            };

        public static readonly StockExchangeResponse DefaultResponse = new()
                                                                       {
                                                                           Id         = Constant.Id,
                                                                           Name       = Constant.CompanyName,
                                                                           Acronym    = Constant.Acronym,
                                                                           MIC        = Constant.MIC,
                                                                           Polity     = Constant.CountryName,
                                                                           TimeZone   = Constant.TimeZone,
                                                                           Currency   = Currency.DefaultSimpleResponse,
                                                                           CreatedAt  = Constant.CreatedAt,
                                                                           ModifiedAt = Constant.ModifiedAt
                                                                       };
    }
}
