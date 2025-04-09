using System.Diagnostics;

using Bank.Application.Domain;
using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Database.Seeders;

using SecurityModel = Security;

public static partial class Seeder
{
    public static class Option
    {
        public static readonly SecurityModel AppleCallOption = new()
                                                               {
                                                                   Id                = Guid.Parse("c426aed1-9c27-4da1-aa51-6cf1045528d8"),
                                                                   OptionType        = OptionType.Call,
                                                                   StrikePrice       = 180.00m,
                                                                   ImpliedVolatility = 0.25m,
                                                                   OpenInterest      = 15234,
                                                                   SettlementDate    = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                                                                   Name              = "AAPL Mar 2024 180 Call",
                                                                   Ticker            = "AAPL240315C00180000",
                                                                   StockExchangeId   = StockExchange.Nasdaq.Id,
                                                                   SecurityType      = SecurityType.Option,
                                                               };

        public static readonly SecurityModel TeslaPutOption = new()
                                                              {
                                                                  Id                = Guid.Parse("d2accd8e-ed9a-4a51-96c8-f38a857bcc7f"),
                                                                  OptionType        = OptionType.Put,
                                                                  StrikePrice       = 240.00m,
                                                                  ImpliedVolatility = 0.45m,
                                                                  OpenInterest      = 8765,
                                                                  SettlementDate    = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)),
                                                                  Name              = "TSLA Feb 2024 240 Put",
                                                                  Ticker            = "TSLA240215P00240000",
                                                                  StockExchangeId   = StockExchange.Nasdaq.Id,
                                                                  SecurityType      = SecurityType.Option,
                                                              };

        public static readonly SecurityModel MicrosoftCallOption = new()
                                                                   {
                                                                       Id                = Guid.Parse("c63d39fd-773b-4fb1-bfae-ae5af3ba8696"),
                                                                       OptionType        = OptionType.Call,
                                                                       StrikePrice       = 340.00m,
                                                                       ImpliedVolatility = 0.20m,
                                                                       OpenInterest      = 12456,
                                                                       SettlementDate    = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                                                                       Name              = "MSFT Jan 2024 340 Call",
                                                                       Ticker            = "MSFT240119C00340000",
                                                                       StockExchangeId   = StockExchange.ClearStreet.Id,
                                                                       SecurityType      = SecurityType.Option,
                                                                   };

        public static readonly SecurityModel AmazonPutOption = new()
                                                               {
                                                                   Id                = Guid.Parse("2bca4f12-be42-4d28-8320-11e78b3f4037"),
                                                                   OptionType        = OptionType.Put,
                                                                   StrikePrice       = 130.00m,
                                                                   ImpliedVolatility = 0.30m,
                                                                   OpenInterest      = 9876,
                                                                   SettlementDate    = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(4)),
                                                                   Name              = "AMZN Apr 2024 130 Put",
                                                                   Ticker            = "AMZN240419P00130000",
                                                                   StockExchangeId   = StockExchange.ASX.Id,
                                                                   SecurityType      = SecurityType.Option,
                                                               };
    }
}

public static class OptionSeederExtension
{
    // public static async Task SeedOption(this DatabaseContext context)
    // {
    //     if (context.Securities.Any(security => security.SecurityType == SecurityType.Option))
    //         return;
    //
    //     await context.Securities.AddRangeAsync(Seeder.Option.AmazonPutOption, Seeder.Option.AppleCallOption, Seeder.Option.MicrosoftCallOption, Seeder.Option.TeslaPutOption);
    //
    //     await context.SaveChangesAsync();
    // }

    public static async Task SeedOptions(this DatabaseContext context, HttpClient httpClient)
    {
        if (context.Securities.Any(security => security.SecurityType == SecurityType.Stock))
            return;

        var (apiKey, apiSecret) = Configuration.Security.Stock.ApiKeyAndSecret;

        var request = new HttpRequestMessage
                      {
                          Method     = HttpMethod.Get,
                          RequestUri = new Uri($"{Configuration.Security.Stock.GetAllApi}?status=active&asset_class=us_equity&attributes="),
                          Headers =
                          {
                              { "accept", "application/json" },
                              { "APCA-API-KEY-ID", apiKey },
                              { "APCA-API-SECRET-KEY", apiSecret },
                          },
                      };

        using var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadFromJsonAsync<List<FetchStockResponse>>();

        if (body is null)
            throw new Exception("List of stocks can't be null");

        Console.WriteLine(body);

        var stockExchanges = await context.StockExchanges.ToListAsync();

        var stocks = body.Select(stockResponse =>
                                 {
                                     var exchange = stockExchanges.Find(exchange => exchange.Acronym == stockResponse.StockExchangeAcronym);

                                     return exchange != null && stockResponse.Tradable
                                            ? stockResponse.ToStock(exchange.Id)
                                                           .ToSecurity()
                                            : null;
                                 })
                         .Where(security => security != null)
                         .Select(security => security!)
                         .ToList();

        await context.Securities.AddRangeAsync(stocks);

        await context.SaveChangesAsync();
    }

    public static async Task SeedStockHardcoded(this DatabaseContext context)
    {
        if (context.Securities.Any(security => security.SecurityType == SecurityType.Stock))
            return;

        await context.Securities.AddRangeAsync(Seeder.Stock.Apple, Seeder.Stock.Amazon, Seeder.Stock.Microsoft, Seeder.Stock.Tesla);

        await context.SaveChangesAsync();
    }
}
