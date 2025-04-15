using Bank.ExchangeService.Model;

namespace Bank.ExchangeService.Database.Seeders;

using StockExchangeModel = StockExchange;

public static partial class Seeder
{
    public static class StockExchange
    {
        public static readonly StockExchangeModel Nasdaq = new()
                                                           {
                                                               Id         = Guid.Parse("00b0538d-4c7e-4e3c-b8f8-e101654e57fe"),
                                                               Name       = "Nasdaq",
                                                               Acronym    = "NASDAQ",
                                                               MIC        = "XNAS",
                                                               Polity     = "USA",
                                                               CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                               TimeZone   = TimeSpan.Zero,
                                                               CreatedAt  = DateTime.UtcNow,
                                                               ModifiedAt = DateTime.UtcNow
                                                           };

        public static readonly StockExchangeModel ASX = new()
                                                        {
                                                            Id         = Guid.Parse("f92a6507-e8d5-4cb2-bac5-3b40fdc3bbaf"),
                                                            Name       = "Asx - Trade24",
                                                            Acronym    = "SFE",
                                                            MIC        = "XSFE",
                                                            Polity     = "Australia",
                                                            CurrencyId = Guid.Parse("895ab6f9-8a9a-4532-bea7-b3361d0dc936"), // AUD
                                                            TimeZone   = TimeSpan.Zero,
                                                            CreatedAt  = DateTime.UtcNow,
                                                            ModifiedAt = DateTime.UtcNow
                                                        };

        public static readonly StockExchangeModel EDGADark = new()
                                                             {
                                                                 Id         = Guid.Parse("e0dc796a-c8d8-44f7-9716-407782e870df"),
                                                                 Name       = "Cboe Edga U.s. Equities Exchange Dark",
                                                                 Acronym    = "EDGADARK",
                                                                 MIC        = "EDGD",
                                                                 Polity     = "United States",
                                                                 CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                                 TimeZone   = TimeSpan.Zero,
                                                                 CreatedAt  = DateTime.UtcNow,
                                                                 ModifiedAt = DateTime.UtcNow
                                                             };

        public static readonly StockExchangeModel ClearStreet = new()
                                                                {
                                                                    Id         = Guid.Parse("d5041fff-189f-4aea-bd52-fdc91c0a0483"),
                                                                    Name       = "Clear Street",
                                                                    Acronym    = "CLST",
                                                                    MIC        = "CLST",
                                                                    Polity     = "United States",
                                                                    CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                                    TimeZone   = TimeSpan.Zero,
                                                                    CreatedAt  = DateTime.UtcNow,
                                                                    ModifiedAt = DateTime.UtcNow
                                                                };

        public static readonly StockExchangeModel MarexIreland = new()
                                                                 {
                                                                     Id         = Guid.Parse("d1a9ea53-e940-49f0-b9d6-4145b3902eaf"),
                                                                     Name       = "Marex Spectron Europe Limited - Otf",
                                                                     Acronym    = "MSEL OTF",
                                                                     MIC        = "MSEL",
                                                                     Polity     = "Ireland",
                                                                     CurrencyId = Guid.Parse("6842a5fa-eee4-4438-bcff-5217b6ac6ace"), // Euro
                                                                     TimeZone   = TimeSpan.Zero,
                                                                     CreatedAt  = DateTime.UtcNow,
                                                                     ModifiedAt = DateTime.UtcNow
                                                                 };

        public static readonly StockExchangeModel BorsaItaliana = new()
                                                                  {
                                                                      Id         = Guid.Parse("c2d3a01d-1ec9-4899-b1d2-bcde6a3e139e"),
                                                                      Name       = "Borsa Italiana Equity Mtf",
                                                                      Acronym    = "BITEQMTF",
                                                                      MIC        = "MTAH",
                                                                      Polity     = "Italy",
                                                                      CurrencyId = Guid.Parse("6842a5fa-eee4-4438-bcff-5217b6ac6ace"), // Euro
                                                                      TimeZone   = TimeSpan.Zero,
                                                                      CreatedAt  = DateTime.UtcNow,
                                                                      ModifiedAt = DateTime.UtcNow
                                                                  };

        public static readonly StockExchangeModel ForexMarket = new()
                                                                {
                                                                    Id         = Guid.Parse("a1b2c3d4-e5f6-4718-9a0b-cd1e2f3a4b5c"),
                                                                    Name       = "Global Forex Market",
                                                                    Acronym    = "FOREX",
                                                                    MIC        = "XXXX",
                                                                    Polity     = "International",
                                                                    CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                                    TimeZone   = TimeSpan.Zero,
                                                                    CreatedAt  = DateTime.UtcNow,
                                                                    ModifiedAt = DateTime.UtcNow
                                                                };

        public static readonly StockExchangeModel CME = new()
                                                        {
                                                            Id         = Guid.Parse("c3d8d466-22c0-487c-8247-c229efda2a03"),
                                                            Name       = "Chicago Mercantile Exchange",
                                                            Acronym    = "CME",
                                                            MIC        = "XCME",
                                                            Polity     = "United States",
                                                            CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                            TimeZone   = TimeSpan.FromHours(-6),                             // CST/CDT
                                                            CreatedAt  = DateTime.UtcNow,
                                                            ModifiedAt = DateTime.UtcNow
                                                        };

        public static readonly StockExchangeModel ICE = new()
                                                        {
                                                            Id         = Guid.Parse("bdeb3a2c-831c-44a3-8ce0-81bc7a9c3773"),
                                                            Name       = "Intercontinental Exchange",
                                                            Acronym    = "ICE",
                                                            MIC        = "IEPA",
                                                            Polity     = "United States",
                                                            CurrencyId = Guid.Parse("81bf331a-0a35-4716-ad12-d1d1bcf66627"), // USD
                                                            TimeZone   = TimeSpan.FromHours(-5),                             // EST/EDT
                                                            CreatedAt  = DateTime.UtcNow,
                                                            ModifiedAt = DateTime.UtcNow
                                                        };

        public static readonly StockExchangeModel LME = new()
                                                        {
                                                            Id         = Guid.Parse("030500c4-f824-4662-a77e-799d22863381"),
                                                            Name       = "London Metal Exchange",
                                                            Acronym    = "LME",
                                                            MIC        = "XLME",
                                                            Polity     = "United Kingdom",
                                                            CurrencyId = Guid.Parse("8e8e9283-4ced-4d9e-aa4a-1036d0174c8c"), // GBP
                                                            TimeZone   = TimeSpan.Zero,                                      // GMT/BST
                                                            CreatedAt  = DateTime.UtcNow,
                                                            ModifiedAt = DateTime.UtcNow
                                                        };

        public static readonly StockExchangeModel TOCOM = new()
                                                          {
                                                              Id         = Guid.Parse("0e6b8da5-ef55-442b-af78-7bf8040980e1"),
                                                              Name       = "Tokyo Commodity Exchange",
                                                              Acronym    = "TOCOM",
                                                              MIC        = "XTKM",
                                                              Polity     = "Japan",
                                                              CurrencyId = Guid.Parse("1a77ed84-d984-4410-85ec-ffde69508625"), // JPY
                                                              TimeZone   = TimeSpan.FromHours(9),                              // JST
                                                              CreatedAt  = DateTime.UtcNow,
                                                              ModifiedAt = DateTime.UtcNow
                                                          };
    }
}

public static class StockExchangeSeederExtension
{
    public static async Task SeedStockExchanges(this DatabaseContext context)
    {
        if (context.StockExchanges.Any())
            return;

        await context.StockExchanges.AddRangeAsync(Seeder.StockExchange.Nasdaq, Seeder.StockExchange.ASX, Seeder.StockExchange.EDGADark, Seeder.StockExchange.ClearStreet,
                                                   Seeder.StockExchange.MarexIreland, Seeder.StockExchange.BorsaItaliana, Seeder.StockExchange.ForexMarket,
                                                   Seeder.StockExchange.CME, Seeder.StockExchange.ICE, Seeder.StockExchange.LME, Seeder.StockExchange.TOCOM);

        await context.SaveChangesAsync();
    }
}
