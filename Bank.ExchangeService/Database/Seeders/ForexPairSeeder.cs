using System.Globalization;
using System.Text.Json;
using System.Web;

using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;

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
    private static readonly HashSet<string> s_HighLiquidityPairs = ["EURUSD", "USDJPY", "GBPUSD", "USDCHF", "AUDUSD", "USDCAD", "EURJPY", "GBPJPY"];

    private static readonly HashSet<string> s_MediumLiquidityPairs =
    [
        "EURGBP", "AUDCAD", "GBPAUD", "CHFJPY", "EURCHF", "EURCAD", "AUDJPY"
    ];

    public static async Task SeedForexPairHardcoded(this DatabaseContext context)
    {
        if (context.Securities.Any(security => security.SecurityType == SecurityType.ForexPair))
            return;

        await context.Securities.AddRangeAsync(Seeder.ForexPair.EurJpy, Seeder.ForexPair.GbpAud, Seeder.ForexPair.UsdEur, Seeder.ForexPair.UsdGbp);
        await context.SaveChangesAsync();
    }

    public static async Task SeedForexPair(this DatabaseContext context, HttpClient httpClient, ICurrencyClient currencyClient, ISecurityRepository securityRepository)
    {
        if (context.Securities.Any(security => security.SecurityType == SecurityType.ForexPair))
            return;

        var currencies = await currencyClient.GetAllCurrenciesSimple();

        if (currencies is null)
        {
            Console.WriteLine($"CURRENCY RESPONSE | Bad request");
            return;
        }

        var apiKey     = Configuration.Security.ForexPair.ApiKeyForex;
        var securities = new List<SecurityModel>();

        foreach (var currencyFrom in currencies)
        {
            foreach (var currencyTo in currencies)
            {
                if (currencyFrom.Id == currencyTo.Id)
                    continue;

                var request = new HttpRequestMessage
                              {
                                  Method = HttpMethod.Get,
                                  RequestUri = new Uri($"{Configuration.Security.ForexPair.GetDataApi}?function=CURRENCY_EXCHANGE_RATE&from_currency={
                                      currencyFrom.Code}&to_currency={currencyTo.Code}&apikey={apiKey}"),
                                  Headers =
                                  {
                                      { "accept", "application/json" },
                                  }
                              };

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
                    return;
                }

                var parsed = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                if (!parsed.RootElement.TryGetProperty("Realtime Currency Exchange Rate", out var forexPairElement))
                    continue;

                var body = JsonSerializer.Deserialize<FetchForexPairLatestResponse>(forexPairElement.GetRawText());

                if (body is null)
                {
                    Console.WriteLine($"RESULT IS NULL");
                    return;
                }

                Liquidity liquidity;
                var       ticker        = $"{currencyFrom}{currencyTo}";
                var       reverseTicker = $"{currencyTo}{currencyFrom}";

                if (s_HighLiquidityPairs.Contains(ticker) || s_HighLiquidityPairs.Contains(reverseTicker))
                    liquidity = Liquidity.High;
                else if (s_MediumLiquidityPairs.Contains(ticker) || s_MediumLiquidityPairs.Contains(reverseTicker))
                    liquidity = Liquidity.Medium;
                else
                    liquidity = Liquidity.Low;

                securities.Add(body.ToForexPair(currencyFrom, currencyTo, liquidity, Seeder.StockExchange.ForexMarket.Id)
                                   .ToSecurity());
            }
        }

        Console.WriteLine("Seeding database...");

        await securityRepository.CreateSecurities(securities);

        Console.WriteLine("Completed");
    }

    public static async Task SeedForexPairLatest(this DatabaseContext context, HttpClient httpClient, ICurrencyClient currencyClient, ISecurityRepository securityRepository,
                                                 IQuoteRepository     quoteRepository)
    {
        var currencies = await currencyClient.GetAllCurrenciesSimple();

        if (currencies is null)
        {
            Console.WriteLine($"CURRENCY RESPONSE | Bad request");
            return;
        }

        var forexPairs         = (await securityRepository.FindAll(SecurityType.ForexPair)).Select(security => security.ToForexPair());
        var tickerAndForexPair = forexPairs.ToDictionary(forexPair => forexPair.Ticker, forexPair => forexPair);

        var apiKey = Configuration.Security.ForexPair.ApiKeyForex;
        var quotes = new List<Quote>();

        foreach (var currencyFrom in currencies)
        {
            foreach (var currencyTo in currencies)
            {
                if (currencyFrom.Id == currencyTo.Id)
                    continue;

                var request = new HttpRequestMessage
                              {
                                  Method = HttpMethod.Get,
                                  RequestUri = new Uri($"{Configuration.Security.ForexPair.GetDataApi}?function=CURRENCY_EXCHANGE_RATE&from_currency={
                                      currencyFrom.Code}&to_currency={currencyTo.Code}&apikey={apiKey}"),
                                  Headers =
                                  {
                                      { "accept", "application/json" },
                                  }
                              };

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
                    return;
                }

                var parsed = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                // Console.WriteLine($"CurrencyFrom | {currencyFrom.Code} | CurrencyTo | {currencyTo.Code}");
                // Console.WriteLine(parsed);

                if (!parsed.RootElement.TryGetProperty("Realtime Currency Exchange Rate", out var forexPairElement))
                    continue;
                
                // Console.WriteLine("PASSED");

                var body = JsonSerializer.Deserialize<FetchForexPairLatestResponse>(forexPairElement.GetRawText());

                if (body is null)
                {
                    Console.WriteLine($"RESULT IS NULL");
                    return;
                }

                var forexPairId = tickerAndForexPair[$"{currencyFrom.Code}{currencyTo.Code}"].Id;
                quotes.Add(body.ToQuote(forexPairId));
            }
        }

        Console.WriteLine("Seeding database...");

        await quoteRepository.CreateQuotes(quotes);

        Console.WriteLine("Completed");
    }

    public static async Task SeedForexPairQuotes(this DatabaseContext context, HttpClient httpClient, ICurrencyClient currencyClient, ISecurityRepository securityRepository,
                                                 IQuoteRepository     quoteRepository)
    {
        var forexPairs         = (await securityRepository.FindAll(SecurityType.ForexPair)).Select(security => security.ToForexPair());
        var tickerAndForexPair = forexPairs.ToDictionary(forexPair => forexPair.Ticker, forexPair => forexPair);

        var currencies = await currencyClient.GetAllCurrenciesSimple();

        if (currencies is null)
        {
            Console.WriteLine($"CURRENCY RESPONSE | Bad request");
            return;
        }

        var apiKey = Configuration.Security.ForexPair.ApiKeyForex;
        var quotes = new List<Quote>();

        foreach (var currencyFrom in currencies)
        {
            foreach (var currencyTo in currencies)
            {
                if (currencyFrom.Id == currencyTo.Id)
                    continue;

                var request = new HttpRequestMessage
                              {
                                  Method = HttpMethod.Get,
                                  RequestUri = new Uri($"{Configuration.Security.ForexPair.GetDataApi}?function=FX_DAILY&from_symbol={currencyFrom.Code
                                  }&to_symbol={currencyTo.Code}&apikey={apiKey}"),
                                  Headers =
                                  {
                                      { "accept", "application/json" },
                                  }
                              };

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"RESPONSE | {response.StatusCode} | {await response.Content.ReadAsStringAsync()}");
                    return;
                }

                if (!JsonDocument.Parse(await response.Content.ReadAsStringAsync())
                                 .RootElement.TryGetProperty("Meta Data", out _))
                    continue;

                var body = await response.Content.ReadFromJsonAsync<FetchForexPairQuotesResponse>();

                if (body is null)
                {
                    Console.WriteLine($"RESULT IS NULL");
                    return;
                }

                var forexPairId = tickerAndForexPair[$"{currencyFrom.Code}{currencyTo.Code}"].Id;

                quotes.AddRange(body.Quotes.Select(pair =>
                                                   {
                                                       var date = DateTime.ParseExact(pair.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                                          .ToUniversalTime()
                                                                          .AddHours(2);

                                                       return pair.Value.ToQuote(forexPairId, date);
                                                   }));
            }
        }

        Console.WriteLine("Seeding database...");

        await quoteRepository.CreateQuotes(quotes);

        Console.WriteLine("Completed");
    }
}
