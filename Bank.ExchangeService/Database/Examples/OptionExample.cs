using Bank.Application.Domain;
using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

file static class Values
{
    public static readonly Guid       Id                           = Guid.Parse("102d461a-022a-4d45-9f96-3f6dc267fcfb");
    public const           decimal    StrikePrice                  = 150.0m;
    public const           decimal    ImpliedVolatility            = 0.25m;
    public static readonly DateOnly   SettlementDate               = DateOnly.FromDateTime(DateTime.UtcNow);
    public const           string     Name                         = "AAPL Jun 2024 190 Call";
    public const           string     Ticker                       = "AAPL240615C00190000";
    public static readonly OptionType OptionType                   = OptionType.Call;
    public const           decimal    HighPrice                    = 12.75m;
    public const           decimal    LowPrice                     = 10.10m;
    public const           long       Volume                       = 45000;
    public const           decimal    PriceChange                  = 0.85m;
    public const           decimal    PriceChangeInInterval        = 1.25m;
    public const           decimal    PriceChangePercentInInterval = 0.084m;
    public const           decimal    AskPrice                     = 12.50m;
    public const           decimal    BidPrice                     = 12.25m;
}

public static partial class Example
{
    public static class Option
    {
        public static readonly OptionResponse Response = new()
                                                         {
                                                             Id                           = Values.Id,
                                                             StrikePrice                  = Values.StrikePrice,
                                                             ImpliedVolatility            = Values.ImpliedVolatility,
                                                             SettlementDate               = Values.SettlementDate,
                                                             Name                         = Values.Name,
                                                             Ticker                       = Values.Ticker,
                                                             OptionType                   = Values.OptionType,
                                                             HighPrice                    = Values.HighPrice,
                                                             LowPrice                     = Values.LowPrice,
                                                             Volume                       = Values.Volume,
                                                             PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                             PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                             AskPrice                     = Values.AskPrice,
                                                             BidPrice                     = Values.BidPrice,
                                                             CreatedAt                    = DateTime.UtcNow,
                                                             ModifiedAt                   = DateTime.UtcNow,
                                                             StockExchange                = null!,
                                                             Quotes                       = []
                                                         };

        public static readonly OptionSimpleResponse SimpleResponse = new()
                                                                     {
                                                                         Id                           = Values.Id,
                                                                         StrikePrice                  = Values.StrikePrice,
                                                                         ImpliedVolatility            = Values.ImpliedVolatility,
                                                                         SettlementDate               = Values.SettlementDate,
                                                                         Name                         = Values.Name,
                                                                         Ticker                       = Values.Ticker,
                                                                         OptionType                   = Values.OptionType,
                                                                         HighPrice                    = Values.HighPrice,
                                                                         LowPrice                     = Values.LowPrice,
                                                                         Volume                       = Values.Volume,
                                                                         PriceChange                  = Values.PriceChange,
                                                                         PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                         PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                         AskPrice                     = Values.AskPrice,
                                                                         BidPrice                     = Values.BidPrice,
                                                                         CreatedAt                    = DateTime.UtcNow,
                                                                         ModifiedAt                   = DateTime.UtcNow
                                                                     };

        public static readonly OptionDailyResponse DailyResponse = new()
                                                                   {
                                                                       Id                           = Values.Id,
                                                                       StrikePrice                  = Values.StrikePrice,
                                                                       ImpliedVolatility            = Values.ImpliedVolatility,
                                                                       SettlementDate               = Values.SettlementDate,
                                                                       Name                         = Values.Name,
                                                                       Ticker                       = Values.Ticker,
                                                                       OptionType                   = Values.OptionType,
                                                                       HighPrice                    = Values.HighPrice,
                                                                       LowPrice                     = Values.LowPrice,
                                                                       OpeningPrice                 = 11.20m,
                                                                       ClosePrice                   = 12.30m,
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
