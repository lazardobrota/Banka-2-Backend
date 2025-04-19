using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class FutureContract
    {
        public static readonly FutureContractResponse DeafultResponse = new()
                                                                        {
                                                                            Id                           = Constant.Id,
                                                                            ContractSize                 = Constant.ContractSize,
                                                                            ContractUnit                 = Constant.ContractUnit,
                                                                            SettlementDate               = Constant.CreationDate,
                                                                            Name                         = Constant.FutureContractName,
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

        public static readonly FutureContractSimpleResponse DefaultSimpleResponse = new()
                                                                                    {
                                                                                        Id                           = Constant.Id,
                                                                                        ContractSize                 = Constant.ContractSize,
                                                                                        ContractUnit                 = Constant.ContractUnit,
                                                                                        SettlementDate               = Constant.CreationDate,
                                                                                        Name                         = Constant.FutureContractName,
                                                                                        Ticker                       = Constant.Ticker,
                                                                                        HighPrice                    = Constant.HighPrice,
                                                                                        LowPrice                     = Constant.LowPrice,
                                                                                        Volume                       = Constant.Volume,
                                                                                        PriceChangeInInterval        = Constant.PriceChangeInInterval,
                                                                                        PriceChangePercentInInterval = Constant.PriceChangePercentInInterval,
                                                                                        Price                        = Constant.Price,
                                                                                        CreatedAt                    = Constant.CreatedAt,
                                                                                        ModifiedAt                   = Constant.ModifiedAt
                                                                                    };

        public static readonly FutureContractDailyResponse DefaultDailyResponse = new()
                                                                                  {
                                                                                      Id                           = Constant.Id,
                                                                                      ContractSize                 = Constant.ContractSize,
                                                                                      ContractUnit                 = Constant.ContractUnit,
                                                                                      SettlementDate               = Constant.CreationDate,
                                                                                      Name                         = Constant.FutureContractName,
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
