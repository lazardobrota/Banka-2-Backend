using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Stock
    {
        public static readonly StockResponse DefaultResponse = new()
                                                               {
                                                                   Id                           = Constant.Id,
                                                                   Name                         = Constant.StockName,
                                                                   Ticker                       = Constant.Ticker,
                                                                   HighPrice                    = Constant.HighPrice,
                                                                   LowPrice                     = Constant.LowPrice,
                                                                   AskPrice                     = Constant.AskPrice,
                                                                   BidPrice                     = Constant.BidPrice,
                                                                   Volume                       = Constant.Volume,
                                                                   PriceChangeInInterval        = Constant.PriceChangeInInterval,
                                                                   PriceChangePercentInInterval = Constant.PriceChangePercentInInterval,
                                                                   CreatedAt                    = Constant.CreatedAt,
                                                                   ModifiedAt                   = Constant.ModifiedAt,
                                                                   StockExchange                = StockExchange.DefaultResponse,
                                                                   Quotes                       = [Quote.DefaultSimpleResponse]
                                                               };

        public static readonly StockSimpleResponse DefaultSimpleResponse = new()
                                                                           {
                                                                               Id                           = Constant.Id,
                                                                               Name                         = Constant.StockName,
                                                                               Ticker                       = Constant.Ticker,
                                                                               HighPrice                    = Constant.HighPrice,
                                                                               LowPrice                     = Constant.LowPrice,
                                                                               AskPrice                     = Constant.AskPrice,
                                                                               BidPrice                     = Constant.BidPrice,
                                                                               Volume                       = Constant.Volume,
                                                                               PriceChangeInInterval        = Constant.PriceChangeInInterval,
                                                                               PriceChangePercentInInterval = Constant.PriceChangePercentInInterval,
                                                                               CreatedAt                    = Constant.CreatedAt,
                                                                               ModifiedAt                   = Constant.ModifiedAt
                                                                           };

        public static readonly StockDailyResponse DefaultDailyResponse = new()
                                                                         {
                                                                             Id                           = Constant.Id,
                                                                             Name                         = Constant.StockName,
                                                                             Ticker                       = Constant.Ticker,
                                                                             HighPrice                    = Constant.HighPrice,
                                                                             LowPrice                     = Constant.LowPrice,
                                                                             OpenPrice                    = Constant.OpeningPrice,
                                                                             ClosePrice                   = Constant.ClosePrice,
                                                                             Volume                       = Constant.Volume,
                                                                             PriceChangeInInterval        = Constant.PriceChangeInInterval,
                                                                             PriceChangePercentInInterval = Constant.PriceChangePercentInInterval,
                                                                             CreatedAt                    = Constant.CreatedAt,
                                                                             ModifiedAt                   = Constant.ModifiedAt,
                                                                             StockExchange                = StockExchange.DefaultResponse,
                                                                             Quotes                       = [Quote.DefaultDailySimpleResponse]
                                                                         };
    }
}
