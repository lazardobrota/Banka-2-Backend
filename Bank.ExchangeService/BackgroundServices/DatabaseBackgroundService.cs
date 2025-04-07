using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Database.Seeders;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.BackgroundServices;

public class DatabaseBackgroundService(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
{
    private readonly IServiceProvider   m_ServiceProvider   = serviceProvider;
    private readonly IHttpClientFactory m_HttpClientFactory = httpClientFactory;
    private          IStockRepository   m_StockRepository   = null!;
    private          IQuoteRepository   m_QuoteRepository   = null!;
    private          Timer?             m_StockTimer        = null;

    private DatabaseContext Context =>
    m_ServiceProvider.CreateScope()
                     .ServiceProvider.GetRequiredService<DatabaseContext>();

    public void OnApplicationStarted()
    {
        m_StockRepository = m_ServiceProvider.CreateScope()
                                             .ServiceProvider.GetRequiredService<IStockRepository>();

        m_QuoteRepository = m_ServiceProvider.CreateScope()
                                             .ServiceProvider.GetRequiredService<IQuoteRepository>();

        if (Configuration.Database.CreateDrop)
            Context.Database.EnsureDeletedAsync()
                   .Wait();

        Context.Database.EnsureCreatedAsync()
               .Wait();

        using var client = m_HttpClientFactory.CreateClient();

        Context.SeedStockExchanges()
               .Wait();

        Context.SeedOption()
               .Wait();

        Context.SeedForexPair()
               .Wait();

        Context.SeedFutureContract()
               .Wait();

        Context.SeedStock(client)
               .Wait();

        if (!Context.Quotes.Any())
        {
            Context.SeedQuoteStocks(m_HttpClientFactory.CreateClient(), m_StockRepository, m_QuoteRepository)
                   .Wait();
        }

        InitializeStockTimer();
    }

    public void OnApplicationStopped() { }

    public void InitializeStockTimer()
    {
        m_StockTimer = new Timer(async _ => await FetchLatestStocks(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    private async Task FetchLatestStocks()
    {
        using var scope   = m_ServiceProvider.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await context.SeedQuoteStocksLatest(m_HttpClientFactory.CreateClient(), m_StockRepository, m_QuoteRepository);
    }
}
