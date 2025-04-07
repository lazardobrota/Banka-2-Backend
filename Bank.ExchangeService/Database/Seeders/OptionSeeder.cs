using Bank.Application.Domain;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Database.Seeders;

using OptionModel = Option;

public static partial class Seeder
{
    public static class Option
    {
        public static readonly OptionModel AppleCallOption = new()
                                                             {
                                                                 Id                = Guid.Parse("c426aed1-9c27-4da1-aa51-6cf1045528d8"),
                                                                 OptionType        = OptionType.Call,
                                                                 StrikePrice       = 180.00m,
                                                                 ImpliedVolatility = 0.25m,
                                                                 OpenInterest      = 15234,
                                                                 SettlementDate    = DateTime.UtcNow.AddMonths(3),
                                                                 Name              = "AAPL Mar 2024 180 Call",
                                                                 Ticker            = "AAPL240315C00180000",
                                                                 StockExchangeId   = StockExchange.Nasdaq.Id,
                                                             };

        public static readonly OptionModel TeslaPutOption = new()
                                                            {
                                                                Id                = Guid.Parse("d2accd8e-ed9a-4a51-96c8-f38a857bcc7f"),
                                                                OptionType        = OptionType.Put,
                                                                StrikePrice       = 240.00m,
                                                                ImpliedVolatility = 0.45m,
                                                                OpenInterest      = 8765,
                                                                SettlementDate    = DateTime.UtcNow.AddMonths(2),
                                                                Name              = "TSLA Feb 2024 240 Put",
                                                                Ticker            = "TSLA240215P00240000",
                                                                StockExchangeId   = StockExchange.Nasdaq.Id,
                                                            };

        public static readonly OptionModel MicrosoftCallOption = new()
                                                                 {
                                                                     Id                = Guid.Parse("c63d39fd-773b-4fb1-bfae-ae5af3ba8696"),
                                                                     OptionType        = OptionType.Call,
                                                                     StrikePrice       = 340.00m,
                                                                     ImpliedVolatility = 0.20m,
                                                                     OpenInterest      = 12456,
                                                                     SettlementDate    = DateTime.UtcNow.AddMonths(1),
                                                                     Name              = "MSFT Jan 2024 340 Call",
                                                                     Ticker            = "MSFT240119C00340000",
                                                                     StockExchangeId   = StockExchange.ClearStreet.Id,
                                                                 };

        public static readonly OptionModel AmazonPutOption = new()
                                                             {
                                                                 Id                = Guid.Parse("2bca4f12-be42-4d28-8320-11e78b3f4037"),
                                                                 OptionType        = OptionType.Put,
                                                                 StrikePrice       = 130.00m,
                                                                 ImpliedVolatility = 0.30m,
                                                                 OpenInterest      = 9876,
                                                                 SettlementDate    = DateTime.UtcNow.AddMonths(4),
                                                                 Name              = "AMZN Apr 2024 130 Put",
                                                                 Ticker            = "AMZN240419P00130000",
                                                                 StockExchangeId   = StockExchange.ASX.Id,
                                                             };
    }
}

public static class OptionSeederExtension
{
    public static async Task SeedOption(this DatabaseContext context)
    {
        if (context.Options.Any())
            return;

        await context.Options.AddRangeAsync(Seeder.Option.AmazonPutOption, Seeder.Option.AppleCallOption, Seeder.Option.MicrosoftCallOption, Seeder.Option.TeslaPutOption);

        await context.SaveChangesAsync();
    }
}
