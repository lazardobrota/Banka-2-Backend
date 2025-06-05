using Bank.Application.Domain;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Database.Seeders;
using Bank.ExchangeService.Database.WebSockets;
using Bank.ExchangeService.Repositories;
using Bank.Http.Clients.User;

using Microsoft.AspNetCore.SignalR;

namespace Bank.ExchangeService.BackgroundServices;

public class DatabaseBackgroundService(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, IUserServiceHttpClient userServiceHttpClient)
{
    private readonly IServiceProvider       m_ServiceProvider       = serviceProvider;
    private readonly IHttpClientFactory     m_HttpClientFactory     = httpClientFactory;
    private readonly IUserServiceHttpClient m_UserServiceHttpClient = userServiceHttpClient;
    private          ISecurityRepository    m_SecurityRepository    = null!;
    private          IQuoteRepository       m_QuoteRepository       = null!;
    private          Timer?                 m_SecurityTimer;
    private          bool                   m_IsProcessRunning = false;
    private          int                    m_IterationCount   = 0;

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

        if (Configuration.Application.Profile == Profile.Testing)
        {
            Context.SeedFutureContractHardcoded()
                   .Wait();

            Context.SeedForexPairHardcoded()
                   .Wait();

            Context.SeedStockHardcoded()
                   .Wait();

            Context.SeedOptionHardcoded()
                   .Wait();

            return;
        }

        Console.WriteLine("Wait for 'Seeding Completed' message");

        Context.SeedFutureContractsAndQuotes(m_SecurityRepository, m_QuoteRepository)
               .Wait();

        Context.SeedStock(client)
               .Wait();

        Context.SeedForexPair(client, m_UserServiceHttpClient, m_SecurityRepository)
               .Wait();

        Context.SeedOptionsAndQuotes(client, m_SecurityRepository, m_QuoteRepository)
               .Wait();

        Context.SeedForexPairQuotes(client, m_UserServiceHttpClient, m_SecurityRepository, m_QuoteRepository)
               .Wait();

        Context.SeedStockQuotes(client, m_SecurityRepository, m_QuoteRepository)
               .Wait();

        Console.WriteLine("Seeding Completed");
    }

    public void OnApplicationStopped() { }
}
