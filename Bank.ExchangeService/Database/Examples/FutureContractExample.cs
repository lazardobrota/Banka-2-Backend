using Bank.Application.Domain;
using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

file static class Values
{
    public static readonly Guid         Id                           = Guid.Parse("017475e7-528d-4467-95af-d4545b5a8617");
    public const           int          ContractSize                 = 100;
    public static readonly ContractUnit ContractUnit                 = ContractUnit.Barrel;
    public static readonly DateOnly     SettlementDate               = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(1));
    public const           string       Name                         = "Crude Oil May 2025";
    public const           string       Ticker                       = "CLM25";
    public const           decimal      HighPrice                    = 85.75m;
    public const           decimal      LowPrice                     = 82.10m;
    public const           decimal      AskPrice                     = 84.95m;
    public const           decimal      BidPrice                     = 84.75m;
    public const           long         Volume                       = 895000;
    public const           decimal      PriceChangeInInterval        = 1.35m;
    public const           decimal      PriceChangePercentInInterval = 1.62m;
}

public static partial class Example
{
    public static class FutureContract
    {
        public static readonly FutureContractSimpleResponse SimpleResponse = new()
                                                                             {
                                                                                 Id                           = Values.Id,
                                                                                 ContractSize                 = Values.ContractSize,
                                                                                 ContractUnit                 = Values.ContractUnit,
                                                                                 SettlementDate               = Values.SettlementDate,
                                                                                 Name                         = Values.Name,
                                                                                 Ticker                       = Values.Ticker,
                                                                                 HighPrice                    = Values.HighPrice,
                                                                                 LowPrice                     = Values.LowPrice,
                                                                                 Volume                       = Values.Volume,
                                                                                 PriceChangeInInterval        = Values.PriceChangeInInterval,
                                                                                 PriceChangePercentInInterval = Values.PriceChangePercentInInterval,
                                                                                 Price                        = 84.90m,
                                                                                 CreatedAt                    = DateTime.UtcNow,
                                                                                 ModifiedAt                   = DateTime.UtcNow
                                                                             };

        public static readonly FutureContractResponse Response = new()
                                                                 {
                                                                     Id                           = Values.Id,
                                                                     ContractSize                 = Values.ContractSize,
                                                                     ContractUnit                 = Values.ContractUnit,
                                                                     SettlementDate               = Values.SettlementDate,
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

        public static readonly FutureContractDailyResponse DailyResponse = new()
                                                                           {
                                                                               Id                           = Values.Id,
                                                                               ContractSize                 = Values.ContractSize,
                                                                               ContractUnit                 = Values.ContractUnit,
                                                                               SettlementDate               = Values.SettlementDate,
                                                                               Name                         = Values.Name,
                                                                               Ticker                       = Values.Ticker,
                                                                               HighPrice                    = Values.HighPrice,
                                                                               LowPrice                     = Values.LowPrice,
                                                                               OpenPrice                    = 83.00m,
                                                                               ClosePrice                   = 84.90m,
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
