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
                   HighPrice  = quote.HighPrice,
                   LowPrice   = quote.LowPrice,
                   Volume     = quote.Volume,
                   CreatedAt  = quote.CreatedAt,
                   ModifiedAt = quote.ModifiedAt,
                   AskPrice   = quote.AskPrice,
                   BidPrice   = quote.BidPrice,
               };
    }

    public static QuoteLatestSimpleResponse ToLatestSimpleResponse(this Quote quote, string securityTicker)
    {
        return new QuoteLatestSimpleResponse
               {
                   SecurityTicker = securityTicker,
                   AskPrice       = quote.AskPrice,
                   BidPrice       = quote.BidPrice,
                   HighPrice      = quote.HighPrice,
                   LowPrice       = quote.LowPrice,
                   Volume         = quote.Volume,
                   CreatedAt      = quote.CreatedAt,
                   ModifiedAt     = quote.ModifiedAt,
               };
    }

    public static QuoteDailySimpleResponse ToDailySimpleResponse(this DailyQuote quote)
    {
        return new QuoteDailySimpleResponse
               {
                   HighPrice  = quote.HighPrice,
                   LowPrice   = quote.LowPrice,
                   Volume     = quote.Volume,
                   CreatedAt  = DateOnly.FromDateTime(quote.Date),
                   ModifiedAt = DateOnly.FromDateTime(quote.Date),
                   OpenPrice  = quote.OpeningPrice,
                   ClosePrice = quote.ClosePrice,
               };
    }

    public static QuoteSimpleResponse ToChartSimpleResponse(this Quote quote)
    {
        return new QuoteSimpleResponse()
               {
                   Id         = quote.Id,
                   HighPrice  = quote.HighPrice,
                   LowPrice   = quote.LowPrice,
                   Volume     = quote.Volume,
                   CreatedAt  = quote.CreatedAt,
                   ModifiedAt = quote.ModifiedAt,
                   AskPrice   = quote.AskPrice,
                   BidPrice   = quote.BidPrice,
               };
    }

    public static Quote ToQuote(this FetchStockBarOneResponse stockBarOneResponse, Guid stockId)
    {
        return new Quote
               {
                   Id           = Guid.NewGuid(),
                   AskPrice     = stockBarOneResponse.ClosePrice,
                   BidPrice     = 0,
                   SecurityId   = stockId,
                   HighPrice    = stockBarOneResponse.HighPrice,
                   LowPrice     = stockBarOneResponse.LowPrice,
                   Volume       = stockBarOneResponse.NumberOfTradesInInterval,
                   CreatedAt    = stockBarOneResponse.Date,
                   ModifiedAt   = stockBarOneResponse.Date,
                   ClosePrice   = stockBarOneResponse.ClosePrice,
                   OpeningPrice = stockBarOneResponse.OpeningPrice
               };
    }

    public static Quote ToQuote(this FetchStockSnapshotResponse stockSnapshotResponse, Guid stockId)
    {
        return new Quote
               {
                   Id           = Guid.NewGuid(),
                   SecurityId   = stockId,
                   AskPrice     = stockSnapshotResponse.LatestQuote!.AskPrice,
                   BidPrice     = stockSnapshotResponse.LatestQuote!.BidPrice,
                   HighPrice    = stockSnapshotResponse.DailyBar!.HighPrice,
                   LowPrice     = stockSnapshotResponse.DailyBar!.LowPrice,
                   Volume       = stockSnapshotResponse.MinuteBar!.NumberOfSharesInInterval,
                   CreatedAt    = stockSnapshotResponse.LatestQuote!.Date,
                   ModifiedAt   = stockSnapshotResponse.LatestQuote!.Date,
                   ClosePrice   = stockSnapshotResponse.DailyBar!.ClosePrice,
                   OpeningPrice = stockSnapshotResponse.DailyBar!.OpeningPrice,
               };
    }

    public static Quote ToQuote(this FetchForexPairQuoteResponse fetchForexPairQuote, Guid forexPairId, DateTime date)
    {
        return new Quote
               {
                   Id           = Guid.NewGuid(),
                   SecurityId   = forexPairId,
                   AskPrice     = fetchForexPairQuote.Close,
                   BidPrice     = 0,
                   HighPrice    = fetchForexPairQuote.High,
                   LowPrice     = fetchForexPairQuote.Low,
                   Volume       = 0,
                   CreatedAt    = date.ToUniversalTime(),
                   ModifiedAt   = date.ToUniversalTime(),
                   ClosePrice   = fetchForexPairQuote.Close,
                   OpeningPrice = fetchForexPairQuote.Open
               };
    }

    public static Quote ToQuote(this FetchForexPairLatestResponse fetchForexPairLatest, Guid forexPairId)
    {
        return new Quote
               {
                   Id           = Guid.NewGuid(),
                   SecurityId   = forexPairId,
                   AskPrice     = fetchForexPairLatest.AskPrice,
                   BidPrice     = fetchForexPairLatest.BidPrice,
                   HighPrice    = 0,
                   LowPrice     = 0,
                   Volume       = 0,
                   CreatedAt    = fetchForexPairLatest.Date.ToUniversalTime(),
                   ModifiedAt   = fetchForexPairLatest.Date.ToUniversalTime(),
                   ClosePrice   = 0,
                   OpeningPrice = 0
               };
    }

    public static Quote ToQuote(this FetchOptionOneResponse optionResponse, Guid optionId)
    {
        return new Quote
               {
                   Id                = Guid.NewGuid(),
                   SecurityId        = optionId,
                   AskPrice          = optionResponse.LatestQuote!.AskPrice,
                   BidPrice          = optionResponse.LatestQuote!.BidPrice,
                   HighPrice         = optionResponse.DailyBar!.HighPrice,
                   LowPrice          = optionResponse.DailyBar!.LowPrice,
                   Volume            = optionResponse.DailyBar!.Volume,
                   CreatedAt         = optionResponse.DailyBar!.TimeStamp,
                   ModifiedAt        = optionResponse.DailyBar!.TimeStamp,
                   ImpliedVolatility = optionResponse.ImpliedVolatility,
                   OpeningPrice      = optionResponse.DailyBar.OpeningPrice,
                   ClosePrice        = optionResponse.DailyBar.ClosingPrice
               };
    }
}
