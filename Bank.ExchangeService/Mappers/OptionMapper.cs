using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class OptionMapper
{
    public static OptionResponse ToResponse(this Option option, CurrencySimpleResponse currencySimpleResponse)
    {
        return new OptionResponse
               {
                   Id                           = option.Id,
                   StrikePrice                  = option.StrikePrice,
                   ImpliedVolatility            = option.ImpliedVolatility,
                   OpenInterest                 = option.OpenInterest,
                   SettlementDate               = option.SettlementDate,
                   Name                         = option.Name,
                   Ticker                       = option.Ticker,
                   StockExchange                = option.StockExchange!.ToResponse(currencySimpleResponse),
                   OptionType                   = option.OptionType,
                   HighPrice                    = option.HighPrice,
                   LowPrice                     = option.LowPrice,
                   Volume                       = option.Volume,
                   PriceChangeInInterval        = option.PriceChange,
                   PriceChangePercentInInterval = option.PriceChangePercent,
                   Price                        = option.Price,
                   CreatedAt                    = option.CreatedAt,
                   ModifiedAt                   = option.ModifiedAt,
                   SortedQuotes = option.Quotes.Select(quote => quote.ToSimpleResponse())
                                        .ToList(),
               };
    }

    public static OptionSimpleResponse ToSimpleResponse(this Option option)
    {
        return new OptionSimpleResponse
               {
                   Id                           = option.Id,
                   StrikePrice                  = option.StrikePrice,
                   ImpliedVolatility            = option.ImpliedVolatility,
                   OpenInterest                 = option.OpenInterest,
                   SettlementDate               = option.SettlementDate,
                   Name                         = option.Name,
                   Ticker                       = option.Ticker,
                   OptionType                   = option.OptionType,
                   HighPrice                    = option.HighPrice,
                   LowPrice                     = option.LowPrice,
                   Volume                       = option.Volume,
                   PriceChange                  = option.PriceChange,
                   Price                        = option.Price,
                   CreatedAt                    = option.CreatedAt,
                   ModifiedAt                   = option.ModifiedAt,
                   PriceChangeInInterval        = option.PriceChange,
                   PriceChangePercentInInterval = option.PriceChangePercent,
               };
    }
}
