using System.Web;

using Bank.Application.Domain;
using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database.WebSockets;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;

using Microsoft.AspNetCore.SignalR;

namespace Bank.ExchangeService.Database.Seeders;

using QuoteModel = Quote;

public static partial class Seeder
{
    public static class Quote
    {
        public static readonly QuoteModel StockAppleQuote = new()
                                                            {
                                                                Id           = Guid.Parse("a17ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                SecurityId   = Stock.Apple.Id,
                                                                AskPrice     = 175.43m,
                                                                BidPrice     = 174.43m,
                                                                HighPrice    = 176.98m,
                                                                LowPrice     = 174.21m,
                                                                Volume       = 52436789,
                                                                CreatedAt    = DateTime.UtcNow,
                                                                ModifiedAt   = DateTime.UtcNow,
                                                                ClosePrice   = 0,
                                                                OpeningPrice = 0,
                                                            };

        public static readonly QuoteModel StockMicrosoftQuote = new()
                                                                {
                                                                    Id           = Guid.Parse("b17ac10b-58cc-4372-a567-0e02b2c3d480"),
                                                                    SecurityId   = Stock.Microsoft.Id,
                                                                    AskPrice     = 338.11m,
                                                                    BidPrice     = 337.11m,
                                                                    HighPrice    = 339.54m,
                                                                    LowPrice     = 336.77m,
                                                                    Volume       = 23567890,
                                                                    CreatedAt    = DateTime.UtcNow,
                                                                    ModifiedAt   = DateTime.UtcNow,
                                                                    ClosePrice   = 0,
                                                                    OpeningPrice = 0,
                                                                };

        public static readonly QuoteModel StockTeslaQuote = new()
                                                            {
                                                                Id           = Guid.Parse("c17ac10b-58cc-4372-a567-0e02b2c3d481"),
                                                                SecurityId   = Stock.Tesla.Id,
                                                                AskPrice     = 242.68m,
                                                                BidPrice     = 241.68m,
                                                                HighPrice    = 245.33m,
                                                                LowPrice     = 240.12m,
                                                                Volume       = 128907654,
                                                                CreatedAt    = DateTime.UtcNow,
                                                                ModifiedAt   = DateTime.UtcNow,
                                                                ClosePrice   = 0,
                                                                OpeningPrice = 0,
                                                            };

        public static readonly QuoteModel StockAmazonQuote = new()
                                                             {
                                                                 Id           = Guid.Parse("d17ac10b-58cc-4372-a567-0e02b2c3d482"),
                                                                 SecurityId   = Stock.Amazon.Id,
                                                                 AskPrice     = 129.96m,
                                                                 BidPrice     = 128.96m,
                                                                 HighPrice    = 131.25m,
                                                                 LowPrice     = 128.88m,
                                                                 Volume       = 45678901,
                                                                 CreatedAt    = DateTime.UtcNow,
                                                                 ModifiedAt   = DateTime.UtcNow,
                                                                 ClosePrice   = 0,
                                                                 OpeningPrice = 0,
                                                             };

        public static readonly QuoteModel StockAppleQuoteYesterday = new()
                                                                     {
                                                                         Id           = Guid.Parse("a27ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                         SecurityId   = Stock.Apple.Id,
                                                                         AskPrice     = 174.21m,
                                                                         BidPrice     = 173.21m,
                                                                         HighPrice    = 175.10m,
                                                                         LowPrice     = 173.45m,
                                                                         Volume       = 48923451,
                                                                         CreatedAt    = DateTime.UtcNow.AddDays(-1),
                                                                         ModifiedAt   = DateTime.UtcNow.AddDays(-1),
                                                                         ClosePrice   = 0,
                                                                         OpeningPrice = 0,
                                                                     };

        public static readonly QuoteModel StockAppleQuote2DaysAgo = new()
                                                                    {
                                                                        Id           = Guid.Parse("a37ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        SecurityId   = Stock.Apple.Id,
                                                                        AskPrice     = 172.88m,
                                                                        BidPrice     = 173.88m,
                                                                        HighPrice    = 173.95m,
                                                                        LowPrice     = 171.96m,
                                                                        Volume       = 51234567,
                                                                        CreatedAt    = DateTime.UtcNow.AddDays(-2),
                                                                        ModifiedAt   = DateTime.UtcNow.AddDays(-2),
                                                                        ClosePrice   = 0,
                                                                        OpeningPrice = 0,
                                                                    };

        public static readonly QuoteModel StockAppleQuote3DaysAgo = new()
                                                                    {
                                                                        Id           = Guid.Parse("a47ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        SecurityId   = Stock.Apple.Id,
                                                                        AskPrice     = 168.45m,
                                                                        BidPrice     = 167.45m,
                                                                        HighPrice    = 169.87m,
                                                                        LowPrice     = 167.23m,
                                                                        Volume       = 47891234,
                                                                        CreatedAt    = DateTime.UtcNow.AddDays(-3),
                                                                        ModifiedAt   = DateTime.UtcNow.AddDays(-3),
                                                                        ClosePrice   = 0,
                                                                        OpeningPrice = 0,
                                                                    };

        public static readonly QuoteModel StockAppleQuote4DaysAgo = new()
                                                                    {
                                                                        Id           = Guid.Parse("a57ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                                        SecurityId   = Stock.Apple.Id,
                                                                        AskPrice     = 162.33m,
                                                                        BidPrice     = 161.33m,
                                                                        HighPrice    = 164.12m,
                                                                        LowPrice     = 161.78m,
                                                                        Volume       = 45678901,
                                                                        CreatedAt    = DateTime.UtcNow.AddDays(-4),
                                                                        ModifiedAt   = DateTime.UtcNow.AddDays(-4),
                                                                        ClosePrice   = 0,
                                                                        OpeningPrice = 0,
                                                                    };

        public static readonly QuoteModel ForexPairUsdEurLatest = new()
                                                                  {
                                                                      Id           = Guid.Parse("40f83733-e150-41a7-9bfb-21a18f9cb857"),
                                                                      SecurityId   = ForexPair.UsdEur.Id,
                                                                      AskPrice     = 1.0921m,
                                                                      BidPrice     = 1.0901m,
                                                                      HighPrice    = 1.0945m,
                                                                      LowPrice     = 1.0898m,
                                                                      CreatedAt    = DateTime.UtcNow,
                                                                      ModifiedAt   = DateTime.UtcNow,
                                                                      Volume       = 123121,
                                                                      ClosePrice   = 0,
                                                                      OpeningPrice = 0
                                                                  };

        public static readonly QuoteModel ForexPairUsdEurYesterday = new()
                                                                     {
                                                                         Id           = Guid.Parse("79a03d9d-c990-4dfd-8997-494e5cc0905a"),
                                                                         SecurityId   = ForexPair.UsdEur.Id,
                                                                         AskPrice     = 1.0898m,
                                                                         BidPrice     = 1.0888m,
                                                                         HighPrice    = 1.0925m,
                                                                         LowPrice     = 1.0876m,
                                                                         CreatedAt    = DateTime.UtcNow.AddDays(-1),
                                                                         ModifiedAt   = DateTime.UtcNow.AddDays(-1),
                                                                         Volume       = 42919,
                                                                         ClosePrice   = 0,
                                                                         OpeningPrice = 0
                                                                     };

        public static readonly QuoteModel ForexPairEurJpyQuote = new()
                                                                 {
                                                                     Id           = Guid.Parse("29301e3c-9181-4a0c-8b56-6c1a3d3ed271"),
                                                                     SecurityId   = ForexPair.EurJpy.Id,
                                                                     AskPrice     = 1.0876m,
                                                                     BidPrice     = 1.0856m,
                                                                     HighPrice    = 1.0901m,
                                                                     LowPrice     = 1.0845m,
                                                                     CreatedAt    = DateTime.UtcNow,
                                                                     ModifiedAt   = DateTime.UtcNow,
                                                                     Volume       = 231321,
                                                                     ClosePrice   = 0,
                                                                     OpeningPrice = 0
                                                                 };

        public static readonly QuoteModel FutureContractCrudeOilLatest = new()
                                                                         {
                                                                             Id           = Guid.Parse("8b52b6fc-530b-4063-b2ec-e7bd808db906"),
                                                                             SecurityId   = FutureContract.CrudeOilFuture.Id,
                                                                             AskPrice     = 72.45m,
                                                                             BidPrice     = 72.15m,
                                                                             HighPrice    = 73.21m,
                                                                             LowPrice     = 71.98m,
                                                                             CreatedAt    = DateTime.UtcNow,
                                                                             ModifiedAt   = DateTime.UtcNow,
                                                                             Volume       = 223321,
                                                                             ClosePrice   = 0,
                                                                             OpeningPrice = 0
                                                                         };

        public static readonly QuoteModel FutureContractCrudeOilYesterday = new()
                                                                            {
                                                                                Id           = Guid.Parse("41ea6c56-afaf-4cfc-9f46-6824f5757fe9"),
                                                                                SecurityId   = FutureContract.CrudeOilFuture.Id,
                                                                                AskPrice     = 71.98m,
                                                                                BidPrice     = 71.58m,
                                                                                HighPrice    = 72.54m,
                                                                                LowPrice     = 71.45m,
                                                                                CreatedAt    = DateTime.UtcNow.AddDays(-1),
                                                                                ModifiedAt   = DateTime.UtcNow.AddDays(-1),
                                                                                Volume       = 132215,
                                                                                ClosePrice   = 0,
                                                                                OpeningPrice = 0
                                                                            };

        public static readonly QuoteModel FutureContractCrudeOilLastWeek = new()
                                                                           {
                                                                               Id           = Guid.Parse("b41e80b5-cd29-4922-b5c5-bec0bbe1064a"),
                                                                               SecurityId   = FutureContract.CrudeOilFuture.Id,
                                                                               AskPrice     = 70.89m,
                                                                               BidPrice     = 70.69m,
                                                                               HighPrice    = 71.34m,
                                                                               LowPrice     = 70.21m,
                                                                               Volume       = 187654,
                                                                               CreatedAt    = DateTime.UtcNow.AddDays(-7),
                                                                               ModifiedAt   = DateTime.UtcNow.AddDays(-7),
                                                                               ClosePrice   = 0,
                                                                               OpeningPrice = 0,
                                                                           };

        public static readonly QuoteModel OptionAppleCallOptionLatest = new()
                                                                        {
                                                                            Id           = Guid.Parse("ed3ab504-2cb1-431c-84ee-19ac4cdd0885"),
                                                                            SecurityId   = Option.AppleCallOption.Id,
                                                                            AskPrice     = 5.45m,
                                                                            BidPrice     = 5.25m,
                                                                            HighPrice    = 5.65m,
                                                                            LowPrice     = 5.25m,
                                                                            Volume       = 12345,
                                                                            CreatedAt    = DateTime.UtcNow,
                                                                            ModifiedAt   = DateTime.UtcNow,
                                                                            ClosePrice   = 0,
                                                                            OpeningPrice = 0,
                                                                        };

        public static readonly QuoteModel OptionAppleCallOptionYesterday = new()
                                                                           {
                                                                               Id           = Guid.Parse("7e3df1a2-0d2e-4f4a-9d1c-c06c5816752b"),
                                                                               SecurityId   = Option.AppleCallOption.Id,
                                                                               AskPrice     = 5.25m,
                                                                               BidPrice     = 5.15m,
                                                                               HighPrice    = 5.40m,
                                                                               LowPrice     = 5.10m,
                                                                               Volume       = 11234,
                                                                               CreatedAt    = DateTime.UtcNow.AddDays(-1),
                                                                               ModifiedAt   = DateTime.UtcNow.AddDays(-1),
                                                                               ClosePrice   = 0,
                                                                               OpeningPrice = 0,
                                                                           };

        public static readonly QuoteModel OptionAppleCallOptionLastWeek = new()
                                                                          {
                                                                              Id           = Guid.Parse("7e252962-d766-42b8-af47-af09a57d6e93"),
                                                                              SecurityId   = Option.AppleCallOption.Id,
                                                                              AskPrice     = 4.85m,
                                                                              BidPrice     = 4.65m,
                                                                              HighPrice    = 5.00m,
                                                                              LowPrice     = 4.70m,
                                                                              Volume       = 10123,
                                                                              CreatedAt    = DateTime.UtcNow.AddDays(-7),
                                                                              ModifiedAt   = DateTime.UtcNow.AddDays(-7),
                                                                              ClosePrice   = 0,
                                                                              OpeningPrice = 0,
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

    public static async Task SeedQuoteStocksLatest(this DatabaseContext context, HttpClient httpClient, ISecurityRepository securityRepository, IQuoteRepository quoteRepository,
                                                   IHubContext<SecurityHub, ISecurityClient> securityHub)
    {
        var stocks = (await securityRepository.FindAll(SecurityType.Stock)).ToDictionary(stock => stock.Ticker, stock => stock);

        var symbols = string.Join(",", stocks.Values.Select(stock => stock.Ticker)
                                             .ToList());

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["symbols"] = symbols;

        var quotes = new List<QuoteModel>();
        var (apiKey, apiSecret) = Configuration.Security.Keys.AlpacaApiKeyAndSecret;

        var request = new HttpRequestMessage
                      {
                          Method     = HttpMethod.Get,
                          RequestUri = new Uri($"{Configuration.Security.Stock.GetLatest}?{query}"),
                          Headers =
                          {
                              { "accept", "application/json" },
                              { "APCA-API-KEY-ID", apiKey },
                              { "APCA-API-SECRET-KEY", apiSecret },
                          }
                      };

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
            return;
        }

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, FetchStockSnapshotResponse>>();

        if (body is null)
        {
            Console.WriteLine($"RESULT IS NULL");
            return;
        }

        foreach (var pair in body)
        {
            if (pair.Value is not { DailyBar: not null, LatestQuote: not null, MinuteBar: not null })
                continue;

            var quote                     = pair.Value.ToQuote(stocks[pair.Key].Id);
            var quoteLatestSimpleResponse = quote.ToLatestSimpleResponse(pair.Key);
            quotes.Add(quote);

            await securityHub.Clients.Group(quoteLatestSimpleResponse.SecurityTicker)
                             .ReceiveSecurityUpdate(quoteLatestSimpleResponse);
        }

        await quoteRepository.CreateQuotes(quotes);
    }

    public static async Task SeedStockQuotes(this DatabaseContext context, HttpClient httpClient, ISecurityRepository securityRepository, IQuoteRepository quoteRepository)
    {
        if (context.Quotes.Any(quote => quote.Security != null && quote.Security.SecurityType == SecurityType.Stock))
            return;

        var stocks = (await securityRepository.FindAll(SecurityType.Stock)).ToDictionary(stock => stock.Ticker, stock => stock);

        var symbols = string.Join(",", stocks.Values.Select(stock => stock.Ticker)
                                             .ToList());

        string? nextPage = null;
        var     query    = HttpUtility.ParseQueryString(string.Empty);
        var     toDate   = DateTime.Parse(Configuration.Security.Stock.ToDateTime);
        query["symbols"] = symbols;
        query["start"]   = Configuration.Security.Stock.FromDateTime;

        if (toDate.Day < DateTime.Today.Day)
            query["end"] = Configuration.Security.Stock.ToDateTime;

        query["limit"]     = "10000";
        query["timeframe"] = $"{Configuration.Security.Global.HistoryTimeFrameInMinutes}Min";

        var quotes = new List<QuoteModel>();

        do
        {
            if (!string.IsNullOrEmpty(nextPage))
                query["page_token"] = nextPage;
            else
                query.Remove("page_token");

            var (apiKey, apiSecret) = Configuration.Security.Keys.AlpacaApiKeyAndSecret;

            var request = new HttpRequestMessage
                          {
                              Method     = HttpMethod.Get,
                              RequestUri = new Uri($"{Configuration.Security.Stock.GetHistoryApi}?{query}"),
                              Headers =
                              {
                                  { "accept", "application/json" },
                                  { "APCA-API-KEY-ID", apiKey },
                                  { "APCA-API-SECRET-KEY", apiSecret },
                              }
                          };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                break;

            var body = await response.Content.ReadFromJsonAsync<FetchStockBarsResponse>();

            if (body is null)
                break;

            quotes.AddRange(body.Bars.SelectMany(pair =>
                                                 {
                                                     var stockId = stocks[pair.Key].Id;
                                                     return pair.Value.Select(bar => bar.ToQuote(stockId));
                                                 }));

            nextPage = body.NextPage;
        } while (!string.IsNullOrEmpty(nextPage));

        await quoteRepository.CreateQuotes(quotes);
    }
}
