using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

file static class Values
{
    public static readonly Guid    Id                           = Guid.Parse("fc26aacf-d46a-4972-96dc-200eea05c807");
    public const           string  Name                         = "Meta Platforms, Inc.";
    public const           string  Ticker                       = "META";
    public const           decimal HighPrice                    = 189.50m;
    public const           decimal LowPrice                     = 185.25m;
    public const           decimal AskPrice                     = 188.90m;
    public const           decimal BidPrice                     = 188.70m;
    public const           long    Volume                       = 15800000;
    public const           decimal PriceChangeInInterval        = 1.35m;
    public const           decimal PriceChangePercentInInterval = 0.72m;
}

public static partial class Example
{
    public static class Stock
    {
        public static readonly StockSimpleResponse SimpleResponse = new()
                                                                    {
                                                                        Id                           = Values.Id,
                                                                        Name                         = Values.Name,
                                                                        Ticker                       = Values.Ticker,
                                                                        HighPrice                    = Values.HighPrice,
                                                                        LowPrice                     = Values.LowPrice,
                                                                        AskPrice                     = Values.AskPrice,
                                                                        BidPrice                     = Values.BidPrice,
                                                                        Volume                       = Values.Volume,
                                                                        PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                        PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                        CreatedAt                    = DateTime.UtcNow,
                                                                        ModifiedAt                   = DateTime.UtcNow
                                                                    };

        public static readonly StockResponse Response = new()
                                                        {
                                                            Id                           = Values.Id,
                                                            Name                         = Values.Name,
                                                            Ticker                       = Values.Ticker,
                                                            HighPrice                    = Values.HighPrice,
                                                            LowPrice                     = Values.LowPrice,
                                                            AskPrice                     = Values.AskPrice,
                                                            BidPrice                     = Values.BidPrice,
                                                            Volume                       = Values.Volume,
                                                            PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                            PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                            CreatedAt                    = DateTime.UtcNow,
                                                            ModifiedAt                   = DateTime.UtcNow,
                                                            StockExchange                = null!,
                                                            Quotes                       = []
                                                        };

        public static readonly StockDailyResponse DailyResponse = new()
                                                                  {
                                                                      Id                           = Values.Id,
                                                                      Name                         = Values.Name,
                                                                      Ticker                       = Values.Ticker,
                                                                      HighPrice                    = Values.HighPrice,
                                                                      LowPrice                     = Values.LowPrice,
                                                                      OpenPrice                    = 186.50m,
                                                                      ClosePrice                   = 188.80m,
                                                                      Volume                       = Values.Volume,
                                                                      PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                      PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                      CreatedAt                    = DateTime.UtcNow,
                                                                      ModifiedAt                   = DateTime.UtcNow,
                                                                      StockExchange                = null!,
                                                                      Quotes                       = []
                                                                  };
    }
}
