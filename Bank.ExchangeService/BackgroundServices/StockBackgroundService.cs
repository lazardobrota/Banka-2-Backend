using System.Web;

using Bank.Application.Domain;
using Bank.Application.Extensions;
using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database.Processors;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;
using Bank.Http.Clients.User;

namespace Bank.ExchangeService.BackgroundServices;

public class StockBackgroundService(
    IEnumerable<IRealtimeProcessor> realtimeProcessors,
    IHttpClientFactory              httpClientFactory,
    IUserServiceHttpClient          userServiceHttpClient,
    ISecurityRepository             securityRepository
)

{
    private          Timer                           m_Timer              = null!;
    private readonly IEnumerable<IRealtimeProcessor> m_RealtimeProcessors = realtimeProcessors;
    private readonly IHttpClientFactory              m_HttpClientFactory  = httpClientFactory;
    private readonly ISecurityRepository             m_SecurityRepository = securityRepository;
    private          Dictionary<string, Security>    m_StockDictionary    = null!;
    private          string                          m_StockSymbols       = string.Empty;

    public async Task OnApplicationStarted(CancellationToken cancellationToken)
    {
        // init
        m_StockDictionary = (await m_SecurityRepository.FindAll(SecurityType.Stock)).ToDictionary(stock => stock.Ticker, stock => stock);

        m_StockSymbols = string.Join(",", m_StockDictionary.Values.Select(stock => stock.Ticker)
                                                           .ToList());

        m_Timer = new Timer(_ => FetchQuotes()
                            .Wait(cancellationToken), null, TimeSpan.FromMinutes(Configuration.Security.Global.LatestTimeFrameInMinutes),
                            TimeSpan.FromMinutes(Configuration.Security.Global.LatestTimeFrameInMinutes));
    }

    private async Task FetchQuotes()
    {
        //fetch
        //todo
        var httpClient = m_HttpClientFactory.CreateClient();

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["symbols"] = m_StockSymbols;

        var quotes = new List<Quote>();

        //old | start

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

        //old | end

        if (!response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, FetchStockSnapshotResponse>>();

        if (body is null)
            return;

        foreach (var pair in body)
        {
            if (pair.Value is not { DailyBar: not null, LatestQuote: not null, MinuteBar: not null })
                continue;

            var quote = pair.Value.ToQuote(m_StockDictionary[pair.Key]);
            quotes.Add(quote);
        }

        await Task.WhenAll(m_RealtimeProcessors.Select(realtimeProcessor => realtimeProcessor.ProcessStockQuotes(quotes))
                                               .ToList());
    }

    public Task OnApplicationStopped(CancellationToken cancellationToken)
    {
        m_Timer.Cancel();

        return Task.CompletedTask;
    }
}
