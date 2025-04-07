using System.Diagnostics;
using System.Web;

using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.Database.Seeders;

using QuoteModel = Quote;

public static partial class Seeder
{
    public static class Quote
    {
        public static readonly QuoteModel StockAppleQuote = new()
                                                            {
                                                                Id         = Guid.Parse("a17ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                StockId    = Stock.Apple.Id,
                                                                Price      = 175.43m,
                                                                HighPrice  = 176.98m,
                                                                LowPrice   = 174.21m,
                                                                Volume     = 52436789,
                                                                CreatedAt  = DateTime.UtcNow,
                                                                ModifiedAt = DateTime.UtcNow,
                                                            };

        public static readonly QuoteModel StockMicrosoftQuote = new()
                                                                {
                                                                    Id         = Guid.Parse("b17ac10b-58cc-4372-a567-0e02b2c3d480"),
                                                                    StockId    = Stock.Microsoft.Id,
                                                                    Price      = 338.11m,
                                                                    HighPrice  = 339.54m,
                                                                    LowPrice   = 336.77m,
                                                                    Volume     = 23567890,
                                                                    CreatedAt  = DateTime.UtcNow,
                                                                    ModifiedAt = DateTime.UtcNow,
                                                                };

        public static readonly QuoteModel StockTeslaQuote = new()
                                                            {
                                                                Id         = Guid.Parse("c17ac10b-58cc-4372-a567-0e02b2c3d481"),
                                                                StockId    = Stock.Tesla.Id,
                                                                Price      = 242.68m,
                                                                HighPrice  = 245.33m,
                                                                LowPrice   = 240.12m,
                                                                Volume     = 128907654,
                                                                CreatedAt  = DateTime.UtcNow,
                                                                ModifiedAt = DateTime.UtcNow,
                                                            };

        public static readonly QuoteModel StockAmazonQuote = new()
                                                             {
                                                                 Id         = Guid.Parse("d17ac10b-58cc-4372-a567-0e02b2c3d482"),
                                                                 StockId    = Stock.Amazon.Id,
                                                                 Price      = 129.96m,
                                                                 HighPrice  = 131.25m,
                                                                 LowPrice   = 128.88m,
                                                                 Volume     = 45678901,
                                                                 CreatedAt  = DateTime.UtcNow,
                                                                 ModifiedAt = DateTime.UtcNow,
                                                             };

        public static readonly QuoteModel StockAppleQuoteYesterday = new()
                                                                     {
                                                                         Id         = Guid.Parse("a27ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                         StockId    = Stock.Apple.Id,
                                                                         Price      = 174.21m,
                                                                         HighPrice  = 175.10m,
                                                                         LowPrice   = 173.45m,
                                                                         Volume     = 48923451,
                                                                         CreatedAt  = DateTime.UtcNow.AddDays(-1),
                                                                         ModifiedAt = DateTime.UtcNow.AddDays(-1),
                                                                     };

        public static readonly QuoteModel StockAppleQuote2DaysAgo = new()
                                                                    {
                                                                        Id         = Guid.Parse("a37ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        StockId    = Stock.Apple.Id,
                                                                        Price      = 172.88m,
                                                                        HighPrice  = 173.95m,
                                                                        LowPrice   = 171.96m,
                                                                        Volume     = 51234567,
                                                                        CreatedAt  = DateTime.UtcNow.AddDays(-2),
                                                                        ModifiedAt = DateTime.UtcNow.AddDays(-2),
                                                                    };

        public static readonly QuoteModel StockAppleQuote3DaysAgo = new()
                                                                    {
                                                                        Id         = Guid.Parse("a47ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        StockId    = Stock.Apple.Id,
                                                                        Price      = 168.45m,
                                                                        HighPrice  = 169.87m,
                                                                        LowPrice   = 167.23m,
                                                                        Volume     = 47891234,
                                                                        CreatedAt  = DateTime.UtcNow.AddDays(-3),
                                                                        ModifiedAt = DateTime.UtcNow.AddDays(-3),
                                                                    };

        public static readonly QuoteModel StockAppleQuote4DaysAgo = new()
                                                                    {
                                                                        Id         = Guid.Parse("a57ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        StockId    = Stock.Apple.Id,
                                                                        Price      = 162.33m,
                                                                        HighPrice  = 164.12m,
                                                                        LowPrice   = 161.78m,
                                                                        Volume     = 45678901,
                                                                        CreatedAt  = DateTime.UtcNow.AddDays(-4),
                                                                        ModifiedAt = DateTime.UtcNow.AddDays(-4),
                                                                    };

        public static readonly QuoteModel ForexPairUsdEurLatest = new()
                                                                  {
                                                                      Id          = Guid.Parse("40f83733-e150-41a7-9bfb-21a18f9cb857"),
                                                                      ForexPairId = ForexPair.UsdEur.Id,
                                                                      Price       = 1.0921m,
                                                                      HighPrice   = 1.0945m,
                                                                      LowPrice    = 1.0898m,
                                                                      CreatedAt   = DateTime.UtcNow,
                                                                      ModifiedAt  = DateTime.UtcNow,
                                                                      Volume      = 123121
                                                                  };

        public static readonly QuoteModel ForexPairUsdEurYesterday = new()
                                                                     {
                                                                         Id          = Guid.Parse("79a03d9d-c990-4dfd-8997-494e5cc0905a"),
                                                                         ForexPairId = ForexPair.UsdEur.Id,
                                                                         Price       = 1.0898m,
                                                                         HighPrice   = 1.0925m,
                                                                         LowPrice    = 1.0876m,
                                                                         CreatedAt   = DateTime.UtcNow.AddDays(-1),
                                                                         ModifiedAt  = DateTime.UtcNow.AddDays(-1),
                                                                         Volume      = 42919
                                                                     };

        public static readonly QuoteModel ForexPairEurJpyQuote = new()
                                                                 {
                                                                     Id          = Guid.Parse("29301e3c-9181-4a0c-8b56-6c1a3d3ed271"),
                                                                     ForexPairId = ForexPair.EurJpy.Id,
                                                                     Price       = 1.0876m,
                                                                     HighPrice   = 1.0901m,
                                                                     LowPrice    = 1.0845m,
                                                                     CreatedAt   = DateTime.UtcNow,
                                                                     ModifiedAt  = DateTime.UtcNow,
                                                                     Volume      = 231321
                                                                 };

        public static readonly QuoteModel FutureContractCrudeOilLatest = new()
                                                                         {
                                                                             Id                = Guid.Parse("8b52b6fc-530b-4063-b2ec-e7bd808db906"),
                                                                             FuturesContractId = FutureContract.CrudeOilFuture.Id,
                                                                             Price             = 72.45m,
                                                                             HighPrice         = 73.21m,
                                                                             LowPrice          = 71.98m,
                                                                             CreatedAt         = DateTime.UtcNow,
                                                                             ModifiedAt        = DateTime.UtcNow,
                                                                             Volume            = 223321
                                                                         };

        public static readonly QuoteModel FutureContractCrudeOilYesterday = new()
                                                                            {
                                                                                Id                = Guid.Parse("41ea6c56-afaf-4cfc-9f46-6824f5757fe9"),
                                                                                FuturesContractId = FutureContract.CrudeOilFuture.Id,
                                                                                Price             = 71.98m,
                                                                                HighPrice         = 72.54m,
                                                                                LowPrice          = 71.45m,
                                                                                CreatedAt         = DateTime.UtcNow.AddDays(-1),
                                                                                ModifiedAt        = DateTime.UtcNow.AddDays(-1),
                                                                                Volume            = 132215
                                                                            };

        public static readonly QuoteModel FutureContractCrudeOilLastWeek = new()
                                                                           {
                                                                               Id                = Guid.Parse("b41e80b5-cd29-4922-b5c5-bec0bbe1064a"),
                                                                               FuturesContractId = FutureContract.CrudeOilFuture.Id,
                                                                               Price             = 70.89m,
                                                                               HighPrice         = 71.34m,
                                                                               LowPrice          = 70.21m,
                                                                               Volume            = 187654,
                                                                               CreatedAt         = DateTime.UtcNow.AddDays(-7),
                                                                               ModifiedAt        = DateTime.UtcNow.AddDays(-7),
                                                                           };

        public static readonly QuoteModel OptionAppleCallOptionLatest = new()
                                                                        {
                                                                            Id         = Guid.Parse("ed3ab504-2cb1-431c-84ee-19ac4cdd0885"),
                                                                            OptionId   = Option.AppleCallOption.Id,
                                                                            Price      = 5.45m,
                                                                            HighPrice  = 5.65m,
                                                                            LowPrice   = 5.25m,
                                                                            Volume     = 12345,
                                                                            CreatedAt  = DateTime.UtcNow,
                                                                            ModifiedAt = DateTime.UtcNow,
                                                                        };

        public static readonly QuoteModel OptionAppleCallOptionYesterday = new()
                                                                           {
                                                                               Id         = Guid.Parse("7e3df1a2-0d2e-4f4a-9d1c-c06c5816752b"),
                                                                               OptionId   = Option.AppleCallOption.Id,
                                                                               Price      = 5.25m,
                                                                               HighPrice  = 5.40m,
                                                                               LowPrice   = 5.10m,
                                                                               Volume     = 11234,
                                                                               CreatedAt  = DateTime.UtcNow.AddDays(-1),
                                                                               ModifiedAt = DateTime.UtcNow.AddDays(-1),
                                                                           };

        public static readonly QuoteModel OptionAppleCallOptionLastWeek = new()
                                                                          {
                                                                              Id         = Guid.Parse("7e252962-d766-42b8-af47-af09a57d6e93"),
                                                                              OptionId   = Option.AppleCallOption.Id,
                                                                              Price      = 4.85m,
                                                                              HighPrice  = 5.00m,
                                                                              LowPrice   = 4.70m,
                                                                              Volume     = 10123,
                                                                              CreatedAt  = DateTime.UtcNow.AddDays(-7),
                                                                              ModifiedAt = DateTime.UtcNow.AddDays(-7),
                                                                          };
    }
}

public static class QuoteSeederExtension
{
    public static async Task SeedQuotesHardcoded(this DatabaseContext context)
    {
        if (context.Quotes.Any())
            return;

        await context.Quotes.AddRangeAsync(Seeder.Quote.StockAmazonQuote, Seeder.Quote.StockAppleQuote, Seeder.Quote.StockAppleQuoteYesterday, Seeder.Quote.StockAppleQuote2DaysAgo,
                                           Seeder.Quote.StockAppleQuote3DaysAgo, Seeder.Quote.StockAppleQuote4DaysAgo, Seeder.Quote.StockTeslaQuote,
                                           Seeder.Quote.StockMicrosoftQuote);

        await context.Quotes.AddRangeAsync(Seeder.Quote.ForexPairUsdEurLatest, Seeder.Quote.ForexPairEurJpyQuote, Seeder.Quote.ForexPairUsdEurYesterday);

        await context.Quotes.AddRangeAsync(Seeder.Quote.FutureContractCrudeOilLatest, Seeder.Quote.FutureContractCrudeOilYesterday, Seeder.Quote.FutureContractCrudeOilLastWeek);

        await context.Quotes.AddRangeAsync(Seeder.Quote.OptionAppleCallOptionLatest, Seeder.Quote.OptionAppleCallOptionYesterday, Seeder.Quote.OptionAppleCallOptionLastWeek);

        await context.SaveChangesAsync();
    }

    public static async Task SeedQuoteStocksLatest(this DatabaseContext context, HttpClient httpClient, IStockRepository stockRepository, IQuoteRepository quoteRepository)
    {
        var stocks = (await stockRepository.FindAll()).ToDictionary(stock => stock.Ticker, stock => stock);

        var symbols = string.Join(",", stocks.Values.Select(stock => stock.Ticker)
                                             .ToList());

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["symbols"] = symbols;

        var quotes = new List<QuoteModel>();

        var request = new HttpRequestMessage
                      {
                          Method     = HttpMethod.Get,
                          RequestUri = new Uri($"{Configuration.Security.Stock.GetLatest}?{query}"),
                          Headers =
                          {
                              { "accept", "application/json" },
                              { "APCA-API-KEY-ID", Configuration.Security.AlpacaApiKey },
                              { "APCA-API-SECRET-KEY", Configuration.Security.AlpacaSecretKey },
                          }
                      };

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
            return;
        }

        var body = await response.Content.ReadFromJsonAsync<FetchStockBarLatestResponse>();

        if (body is null)
        {
            Console.WriteLine($"RESULT IS NULL");
            return;
        }

        quotes.AddRange(body.Bars.Select(pair => pair.Value.ToQuote(stocks[pair.Key].Id)));

        await quoteRepository.CreateQuotes(quotes);
    }

    public static async Task SeedQuoteStocks(this DatabaseContext context, HttpClient httpClient, IStockRepository stockRepository, IQuoteRepository quoteRepository)
    {
        var stopwatchFull = new Stopwatch();
        stopwatchFull.Start();

        var stocks = (await stockRepository.FindAll()).ToDictionary(stock => stock.Ticker, stock => stock);

        var symbols = string.Join(",", stocks.Values.Select(stock => stock.Ticker)
                                             .ToList());

        string? nextPage = null;
        var     query    = HttpUtility.ParseQueryString(string.Empty);
        query["symbols"] = symbols;
        query["start"]   = Configuration.Security.Stock.StartTime;
        // query["end"]       = Configuration.Security.Stock.EndTime;
        query["limit"]     = "10000";
        query["timeframe"] = "15Min";

        var quotes = new List<QuoteModel>();

        do
        {
            if (!string.IsNullOrEmpty(nextPage))
                query["page_token"] = nextPage;
            else
                query.Remove("page_token");

            var request = new HttpRequestMessage
                          {
                              Method     = HttpMethod.Get,
                              RequestUri = new Uri($"{Configuration.Security.Stock.GetHistoryApi}?{query}"),
                              Headers =
                              {
                                  { "accept", "application/json" },
                                  { "APCA-API-KEY-ID", Configuration.Security.AlpacaApiKey },
                                  { "APCA-API-SECRET-KEY", Configuration.Security.AlpacaSecretKey },
                              }
                          };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
                break;
            }

            var body = await response.Content.ReadFromJsonAsync<FetchStockBarResponse>();

            if (body is null)
            {
                Console.WriteLine($"RESULT IS NULL");
                break;
            }

            quotes.AddRange(body.Bars.SelectMany(pair =>
                                                 {
                                                     var stockId = stocks[pair.Key].Id;
                                                     return pair.Value.Select(bar => bar.ToQuote(stockId));
                                                 }));

            nextPage = body.NextPage;
        } while (!string.IsNullOrEmpty(nextPage));

        Console.WriteLine("Please wait for database to seed...");

        await quoteRepository.CreateQuotes(quotes);

        stopwatchFull.Stop();

        Console.WriteLine($"SeedQuoteStocks | All | Time Elapsed: {stopwatchFull.ElapsedMilliseconds}ms");
    }
}
