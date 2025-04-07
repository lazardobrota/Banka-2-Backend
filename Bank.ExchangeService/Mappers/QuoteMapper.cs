using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class QuoteMapper
{
    public static QuoteSimpleResponse ToSimpleResponse(this Quote quote)
    {
        return new QuoteSimpleResponse
               {
                   Id                           = quote.Id,
                   Price                        = quote.Price,
                   HighPrice                    = quote.HighPrice,
                   LowPrice                     = quote.LowPrice,
                   Volume                       = quote.Volume,
                   CreatedAt                    = quote.CreatedAt,
                   ModifiedAt                   = quote.ModifiedAt,
               };
    }

    public static Quote ToQuote(this FetchBarResponse barResponse, Guid stockId)
    {
        return new Quote
               {
                   Id          = Guid.NewGuid(),
                   Price       = barResponse.LatestPrice,
                   StockId     = stockId,
                   HighPrice   = barResponse.HighPrice,
                   LowPrice    = barResponse.LowPrice,
                   Volume      = barResponse.NumberOfTradesInInterval,
                   CreatedAt   = barResponse.Date,
                   ModifiedAt  = barResponse.Date
               };
    }
}
