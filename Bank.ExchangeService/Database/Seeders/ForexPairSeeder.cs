using Bank.Application.Domain;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Database.Seeders;

using SecurityModel = Security;

public static partial class Seeder
{
    public static class ForexPair
    {
        public static readonly SecurityModel UsdEur = new()
                                                      {
                                                          Id              = Guid.Parse("d17ac10b-58cc-4372-a567-0e02b2c3d488"),
                                                          BaseCurrencyId  = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), //USD
                                                          QuoteCurrencyId = Guid.Parse("6842a5fa-eee4-4438-bcff-5217b6ac6ace"), //EUR
                                                          ExchangeRate    = 1.09m,
                                                          Liquidity       = Liquidity.High,
                                                          ContractSize    = 1000,
                                                          Name            = "USD/EUR",
                                                          Ticker          = "USDEUR",
                                                          StockExchangeId = StockExchange.Nasdaq.Id,
                                                          SecurityType    = SecurityType.ForexPair,
                                                      };

        public static readonly SecurityModel UsdGbp = new()
                                                      {
                                                          Id              = Guid.Parse("d27ac10b-58cc-4372-a567-0e02b2c3d489"),
                                                          BaseCurrencyId  = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), //USD
                                                          QuoteCurrencyId = Guid.Parse("8e8e9283-4ced-4d9e-aa4a-1036d0174c8c"), //GBP
                                                          ExchangeRate    = 0.75m,
                                                          Liquidity       = Liquidity.Low,
                                                          ContractSize    = 1000,
                                                          Name            = "USD/GBP",
                                                          Ticker          = "USDGBP",
                                                          StockExchangeId = StockExchange.ASX.Id,
                                                          SecurityType    = SecurityType.ForexPair,
                                                      };

        public static readonly SecurityModel EurJpy = new()
                                                      {
                                                          Id              = Guid.Parse("d37ac10b-58cc-4372-a567-0e02b2c3d490"),
                                                          BaseCurrencyId  = Guid.Parse("6842a5fa-eee4-4438-bcff-5217b6ac6ace"), //EUR
                                                          QuoteCurrencyId = Guid.Parse("1a77ed84-d984-4410-85ec-ffde69508625"), //JPY
                                                          ExchangeRate    = 145.25m,
                                                          Liquidity       = Liquidity.Medium,
                                                          ContractSize    = 1000,
                                                          Name            = "EUR/JPY",
                                                          Ticker          = "EURJPY",
                                                          StockExchangeId = StockExchange.ClearStreet.Id,
                                                          SecurityType    = SecurityType.ForexPair,
                                                      };

        public static readonly SecurityModel GbpAud = new()
                                                      {
                                                          Id              = Guid.Parse("d47ac10b-58cc-4372-a567-0e02b2c3d491"),
                                                          BaseCurrencyId  = Guid.Parse("8e8e9283-4ced-4d9e-aa4a-1036d0174c8c"), //GBP
                                                          QuoteCurrencyId = Guid.Parse("895ab6f9-8a9a-4532-bea7-b3361d0dc936"), //AUD
                                                          ExchangeRate    = 1.85m,
                                                          Liquidity       = Liquidity.High,
                                                          ContractSize    = 1000,
                                                          Name            = "GBP/AUD",
                                                          Ticker          = "GBPAUD",
                                                          StockExchangeId = StockExchange.ASX.Id,
                                                          SecurityType    = SecurityType.ForexPair,
                                                      };
    }
}

public static class ForexPairSeederExtension
{
    public static async Task SeedForexPair(this DatabaseContext context)
    {
        if (context.Securities.Any(security => security.SecurityType == SecurityType.ForexPair))
            return;

        await context.Securities.AddRangeAsync(Seeder.ForexPair.EurJpy, Seeder.ForexPair.GbpAud, Seeder.ForexPair.UsdEur, Seeder.ForexPair.UsdGbp);
        await context.SaveChangesAsync();
    }
}
