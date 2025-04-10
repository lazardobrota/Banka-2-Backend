using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class QuoteMapper
{
    public static QuoteSimpleResponse ToSimpleResponse(this Quote quote)
    {
        return new QuoteSimpleResponse
               {
                   Id         = quote.Id,
                   Price      = quote.Price,
                   HighPrice  = quote.HighPrice,
                   LowPrice   = quote.LowPrice,
                   Volume     = quote.Volume,
                   CreatedAt  = quote.CreatedAt,
                   ModifiedAt = quote.ModifiedAt,
               };
    }

    public static Quote ToQuote(this FetchBarResponse barResponse, Guid stockId)
    {
        return new Quote
               {
                   Id         = Guid.NewGuid(),
                   Price      = barResponse.LatestPrice,
                   SecurityId = stockId,
                   HighPrice  = barResponse.HighPrice,
                   LowPrice   = barResponse.LowPrice,
                   Volume     = barResponse.NumberOfTradesInInterval,
                   CreatedAt  = barResponse.Date,
                   ModifiedAt = barResponse.Date
               };
    }

    public static Quote ToQuote(this FetchForexPairQuoteResponse fetchForexPairQuote, Guid forexPairId, DateTime date)
    {
        return new Quote
               {
                   Id         = Guid.NewGuid(),
                   SecurityId = forexPairId,
                   Price      = fetchForexPairQuote.Close,
                   HighPrice  = fetchForexPairQuote.High,
                   LowPrice   = fetchForexPairQuote.Low,
                   Volume     = 0,
                   CreatedAt  = date.ToUniversalTime(),
                   ModifiedAt = date.ToUniversalTime()
               };
    }
    
    public static Quote ToQuote(this FetchForexPairLatestResponse fetchForexPairLatest, Guid forexPairId)
    {
        return new Quote
               {
                   Id         = Guid.NewGuid(),
                   SecurityId = forexPairId,
                   Price      = 0, 
                   HighPrice  = fetchForexPairLatest.AskPrice,
                   LowPrice   = fetchForexPairLatest.BidPrice,
                   Volume     = 0,
                   CreatedAt  = fetchForexPairLatest.Date.ToUniversalTime(),
                   ModifiedAt = fetchForexPairLatest.Date.ToUniversalTime()
               };
    }

    public static Quote ToQuote(this FetchOptionOneResponse optionResponse, Guid optionId)
    {
        return new Quote
               {
                   Id         = Guid.NewGuid(),
                   SecurityId = optionId,
                   Price      = optionResponse.LatestQuote!.AskPrice,
                   HighPrice  = optionResponse.DailyBar!.HighPrice,
                   LowPrice   = optionResponse.DailyBar!.LowPrice,
                   Volume     = optionResponse.DailyBar!.Volume,
                   CreatedAt  = optionResponse.DailyBar!.TimeStamp,
                   ModifiedAt = optionResponse.DailyBar!.TimeStamp
               };
    }
}
