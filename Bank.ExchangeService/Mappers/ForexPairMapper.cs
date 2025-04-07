using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class ForexPairMapper
{
    public static ForexPairResponse ToResponse(this ForexPair         forexPair, CurrencySimpleResponse currencyResponse, CurrencySimpleResponse currencyBaseResponse,
                                               CurrencySimpleResponse currencyQuoteResponse)
    {
        return new ForexPairResponse()
               {
                   Id                 = forexPair.Id,
                   Liquidity          = forexPair.Liquidity,
                   ExchangeRate       = forexPair.ExchangeRate,
                   BaseCurrency       = currencyBaseResponse,
                   QuoteCurrency      = currencyQuoteResponse,
                   MaintenanceDecimal = forexPair.MaintenanceDecimal,
                   Name               = forexPair.Name,
                   Ticker             = forexPair.Ticker,
                   StockExchange      = forexPair.StockExchange!.ToResponse(currencyResponse),
                   Quotes = forexPair.SortedQuotes.Select(quote => quote.ToSimpleResponse())
                                     .ToList(),
                   HighPrice   = forexPair.HighPrice,
                   LowPrice    = forexPair.LowPrice,
                   Volume      = forexPair.Volume,
                   PriceChangeInInterval = forexPair.PriceChange,
                   PriceChangePercentInInterval = forexPair.PriceChangePercent,
                   Price       = forexPair.Price,
                   CreatedAt   = forexPair.CreatedAt,
                   ModifiedAt  = forexPair.ModifiedAt
               };
    }

    public static ForexPairSimpleResponse ToSimpleResponse(this ForexPair forexPair, CurrencySimpleResponse currencyBaseResponse, CurrencySimpleResponse currencyQuoteResponse)
    {
        return new ForexPairSimpleResponse()
               {
                   Id                           = forexPair.Id,
                   Liquidity                    = forexPair.Liquidity,
                   ExchangeRate                 = forexPair.ExchangeRate,
                   BaseCurrency                 = currencyBaseResponse,
                   QuoteCurrency                = currencyQuoteResponse,
                   MaintenanceDecimal           = forexPair.MaintenanceDecimal,
                   Name                         = forexPair.Name,
                   Ticker                       = forexPair.Ticker,
                   HighPrice                    = forexPair.HighPrice,
                   LowPrice                     = forexPair.LowPrice,
                   Count                        = forexPair.Volume,
                   PriceChangeInInterval        = forexPair.PriceChange,
                   PriceChangePercentInInterval = forexPair.PriceChangePercent,
                   Price                        = forexPair.Price,
                   CreatedAt                    = forexPair.CreatedAt,
                   ModifiedAt                   = forexPair.ModifiedAt
               };
    }
}
