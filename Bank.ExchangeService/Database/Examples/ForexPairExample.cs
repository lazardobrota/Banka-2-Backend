using Bank.Application.Domain;
using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

file static class Values
{
    public static readonly Guid      Id                           = Guid.Parse("a7f3ce01-8b3d-4c23-b4e7-1427c1e9d67a");
    public static readonly Liquidity Liquidity                    = Liquidity.Medium;
    public const           decimal   MaintenanceDecimal           = 0.0001m;
    public const           decimal   ExchangeRate                 = 1.0932m;
    public const           string    Name                         = "EUR/USD";
    public const           string    Ticker                       = "EURUSD";
    public const           decimal   HighPrice                    = 1.0985m;
    public const           decimal   LowPrice                     = 1.0890m;
    public const           decimal   AskPrice                     = 1.0938m;
    public const           decimal   BidPrice                     = 1.0930m;
    public const           decimal   PriceChangeInInterval        = 0.0025m;
    public const           decimal   PriceChangePercentInInterval = 0.23m;
}

public static partial class Example
{
    public static class ForexPair
    {
        public static readonly ForexPairResponse Response = new()
                                                            {
                                                                Id                           = Values.Id,
                                                                Liquidity                    = Values.Liquidity,
                                                                BaseCurrency                 = null!,
                                                                QuoteCurrency                = null!,
                                                                ExchangeRate                 = Values.ExchangeRate,
                                                                MaintenanceDecimal           = Values.MaintenanceDecimal,
                                                                Name                         = Values.Name,
                                                                Ticker                       = Values.Ticker,
                                                                HighPrice                    = Values.HighPrice,
                                                                LowPrice                     = Values.LowPrice,
                                                                AskPrice                     = Values.AskPrice,
                                                                BidPrice                     = Values.BidPrice,
                                                                PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                CreatedAt                    = DateTime.UtcNow,
                                                                ModifiedAt                   = DateTime.UtcNow,
                                                                StockExchange                = null!,
                                                                Quotes                       = []
                                                            };

        public static readonly ForexPairSimpleResponse SimpleResponse = new()
                                                                        {
                                                                            Id                           = Values.Id,
                                                                            Liquidity                    = Values.Liquidity,
                                                                            BaseCurrency                 = null!,
                                                                            QuoteCurrency                = null!,
                                                                            ExchangeRate                 = Values.ExchangeRate,
                                                                            MaintenanceDecimal           = Values.MaintenanceDecimal,
                                                                            Name                         = Values.Name,
                                                                            Ticker                       = Values.Ticker,
                                                                            HighPrice                    = Values.HighPrice,
                                                                            LowPrice                     = Values.LowPrice,
                                                                            AskPrice                     = Values.AskPrice,
                                                                            BidPrice                     = Values.BidPrice,
                                                                            PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                            PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                            Price                        = 1.0935m,
                                                                            CreatedAt                    = DateTime.UtcNow,
                                                                            ModifiedAt                   = DateTime.UtcNow
                                                                        };

        public static readonly ForexPairDailyResponse DailyResponse = new()
                                                                      {
                                                                          Id                           = Values.Id,
                                                                          Liquidity                    = Values.Liquidity,
                                                                          BaseCurrency                 = null!,
                                                                          QuoteCurrency                = null!,
                                                                          ExchangeRate                 = Values.ExchangeRate,
                                                                          MaintenanceDecimal           = Values.MaintenanceDecimal,
                                                                          Name                         = Values.Name,
                                                                          Ticker                       = Values.Ticker,
                                                                          HighPrice                    = Values.HighPrice,
                                                                          LowPrice                     = Values.LowPrice,
                                                                          OpeningPrice                 = 1.0900m,
                                                                          ClosePrice                   = 1.0935m,
                                                                          PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                          PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                          CreatedAt                    = DateTime.UtcNow,
                                                                          ModifiedAt                   = DateTime.UtcNow,
                                                                          StockExchange                = null!,
                                                                          Quotes                       = []
                                                                      };
    }
}
