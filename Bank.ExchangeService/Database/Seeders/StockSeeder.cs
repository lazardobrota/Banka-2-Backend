using System.Diagnostics;

using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Database.Seeders;

using StockModel = Stock;

public static partial class Seeder
{
    public static class Stock
    {
        public static readonly StockModel Apple = new()
                                                  {
                                                      Id = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                                                      Name            = "Apple Inc.",
                                                      Ticker          = "AAPL",
                                                      StockExchangeId = StockExchange.Nasdaq.Id
                                                  };

        public static readonly StockModel Microsoft = new()
                                                      {
                                                          Id = Guid.Parse("b47ac10b-58cc-4372-a567-0e02b2c3d480"),
                                                          Name            = "Microsoft Corporation",
                                                          Ticker          = "MSFT",
                                                          StockExchangeId = StockExchange.Nasdaq.Id,
                                                      };

        public static readonly StockModel Tesla = new()
                                                  {
                                                      Id = Guid.Parse("c47ac10b-58cc-4372-a567-0e02b2c3d481"),
                                                      Name            = "Tesla, Inc.",
                                                      Ticker          = "TSLA",
                                                      StockExchangeId = StockExchange.ClearStreet.Id
                                                  };

        public static readonly StockModel Amazon = new()
                                                   {
                                                       Id = Guid.Parse("d47ac10b-58cc-4372-a567-0e02b2c3d482"),
                                                       Name            = "Amazon.com, Inc.",
                                                       Ticker          = "AMZN",
                                                       StockExchangeId = StockExchange.ClearStreet.Id,
                                                   };
    }
}

public static class StockSeederExtension
{
    public static async Task SeedStock(this DatabaseContext context, HttpClient httpClient)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();

        if (context.Stocks.Any())
            return;

        var request = new HttpRequestMessage
                      {
                          Method     = HttpMethod.Get,
                          RequestUri = new Uri($"{Configuration.Security.Stock.GetAllApi}?status=active&asset_class=us_equity&attributes="),
                          Headers =
                          {
                              { "accept", "application/json" },
                              { "APCA-API-KEY-ID", Configuration.Security.AlpacaApiKey },
                              { "APCA-API-SECRET-KEY", Configuration.Security.AlpacaSecretKey },
                          },
                      };

        using var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadFromJsonAsync<List<FetchStockResponse>>();

        if (body is null)
            throw new Exception("List of stocks can't be null");

        stopwatch.Stop();

        Console.WriteLine($"SeedStock | API | Time Elapsed: {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();

        Console.WriteLine(body);

        var stockExchanges = await context.StockExchanges.ToListAsync();

        var stocks = body.Select(stockResponse =>
                                 {
                                     var exchange = stockExchanges.Find(exchange => exchange.Acronym == stockResponse.StockExchangeAcronym);
                                     return exchange != null && stockResponse.Tradable ? stockResponse.ToStock(exchange.Id) : null;
                                 })
                         .Where(stock => stock != null)
                         .Select(stock => stock!)
                         .ToList();

        await context.AddRangeAsync(stocks);

        await context.SaveChangesAsync();

        stopwatch.Stop();

        Console.WriteLine($"SeedStock | Save | Time Elapsed: {stopwatch.ElapsedMilliseconds}ms");
    }

    public static async Task SeedStockHardcoded(this DatabaseContext context)
    {
        if (context.Stocks.Any())
            return;

        await context.Stocks.AddRangeAsync(Seeder.Stock.Apple, Seeder.Stock.Amazon, Seeder.Stock.Microsoft, Seeder.Stock.Tesla);

        await context.SaveChangesAsync();
    }
}
