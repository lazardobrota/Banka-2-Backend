using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Database.Seeders;
using Bank.ExchangeService.HttpClients;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.BackgroundServices;

public class DatabaseBackgroundService(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, ICurrencyClient currencyClient)
{
    private readonly IServiceProvider    m_ServiceProvider    = serviceProvider;
    private readonly IHttpClientFactory  m_HttpClientFactory  = httpClientFactory;
    private readonly ICurrencyClient     m_CurrencyClient     = currencyClient;
    private          ISecurityRepository m_SecurityRepository = null!;
    private          IQuoteRepository    m_QuoteRepository    = null!;
    private          Timer?              m_StockTimer;
    private          Timer?              m_ForexPairTimer;
    private          Timer?              m_OptionTimer;

    private DatabaseContext Context =>
    m_ServiceProvider.CreateScope()
                     .ServiceProvider.GetRequiredService<DatabaseContext>();

    public void OnApplicationStarted()
    {
        m_SecurityRepository = m_ServiceProvider.CreateScope()
                                                .ServiceProvider.GetRequiredService<ISecurityRepository>();

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

        // Context.SeedForexPairHardcoded()
        //        .Wait();

        Context.SeedFutureContractHardcoded()
               .Wait();

        Context.SeedForexPair(client, m_CurrencyClient, m_SecurityRepository)
               .Wait();

        Context.SeedStock(client)
               .Wait();

        Context.SeedOptions(client, m_SecurityRepository, m_QuoteRepository)
               .Wait();

        if (!Context.Quotes.Any()) //TODO Need different way to check because SeedOptions fills Quotes
        {
            Context.SeedStockQuotes(m_HttpClientFactory.CreateClient(), m_SecurityRepository, m_QuoteRepository)
                   .Wait();

            Context.SeedForexPairQuotes(m_HttpClientFactory.CreateClient(), m_CurrencyClient, m_SecurityRepository, m_QuoteRepository)
                   .Wait();
        }

        InitializeTimers();
    }

    public void OnApplicationStopped() { }

    public void InitializeTimers()
    {
        m_StockTimer     = new Timer(async _ => await FetchStocksLatest(),    null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        m_ForexPairTimer = new Timer(async _ => await FetchForexPairLatest(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        m_OptionTimer    = new Timer(async _ => await FetchOptionLatest(),    null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    private async Task FetchStocksLatest()
    {
        using var scope              = m_ServiceProvider.CreateScope();
        var       context            = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var       securityRepository = scope.ServiceProvider.GetRequiredService<ISecurityRepository>();
        var       quoteRepository    = scope.ServiceProvider.GetRequiredService<IQuoteRepository>();

        await context.SeedQuoteStocksLatest(m_HttpClientFactory.CreateClient(), securityRepository, quoteRepository);
    }

    private async Task FetchForexPairLatest()
    {
        using var scope              = m_ServiceProvider.CreateScope();
        var       context            = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var       securityRepository = scope.ServiceProvider.GetRequiredService<ISecurityRepository>();
        var       quoteRepository    = scope.ServiceProvider.GetRequiredService<IQuoteRepository>();

        await context.SeedForexPairLatest(m_HttpClientFactory.CreateClient(), m_CurrencyClient, securityRepository, quoteRepository);
    }

    private async Task FetchOptionLatest()
    {
        using var scope              = m_ServiceProvider.CreateScope();
        var       context            = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var       securityRepository = scope.ServiceProvider.GetRequiredService<ISecurityRepository>();
        var       quoteRepository    = scope.ServiceProvider.GetRequiredService<IQuoteRepository>();

        await context.SeedOptionsLatest(m_HttpClientFactory.CreateClient(), securityRepository, quoteRepository);
    }
}
