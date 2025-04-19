using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

file static class Values
{
    public const decimal AskPrice  = 172.34m;
    public const decimal BidPrice  = 172.15m;
    public const decimal HighPrice = 173.00m;
    public const decimal LowPrice  = 170.80m;
    public const long    Volume    = 15000000;
}

public static partial class Example
{
    public static class Quote
    {
        public static readonly QuoteLatestSimpleResponse LatestSimpleResponse = new()
                                                                                {
                                                                                    SecurityTicker = "AAPL",
                                                                                    AskPrice       = Values.AskPrice,
                                                                                    BidPrice       = Values.BidPrice,
                                                                                    HighPrice      = Values.HighPrice,
                                                                                    LowPrice       = Values.LowPrice,
                                                                                    Volume         = Values.Volume,
                                                                                    CreatedAt      = DateTime.UtcNow,
                                                                                    ModifiedAt     = DateTime.UtcNow
                                                                                };

        public static readonly QuoteDailySimpleResponse DailySimpleResponse = new()
                                                                              {
                                                                                  HighPrice  = Values.AskPrice,
                                                                                  LowPrice   = Values.BidPrice,
                                                                                  OpenPrice  = Values.HighPrice,
                                                                                  ClosePrice = Values.LowPrice,
                                                                                  Volume     = Values.Volume,
                                                                                  CreatedAt  = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                                                                                  ModifiedAt = DateOnly.FromDateTime(DateTime.UtcNow.Date)
                                                                              };

        public static readonly QuoteSimpleResponse SimpleResponse = new()
                                                                    {
                                                                        Id         = Guid.Parse("7ab3b0e1-d31f-4f10-8f5a-90d7bfb9f002"),
                                                                        HighPrice  = Values.AskPrice,
                                                                        LowPrice   = Values.BidPrice,
                                                                        AskPrice   = Values.HighPrice,
                                                                        BidPrice   = Values.LowPrice,
                                                                        Volume     = Values.Volume,
                                                                        CreatedAt  = DateTime.UtcNow,
                                                                        ModifiedAt = DateTime.UtcNow
                                                                    };
    }
}
