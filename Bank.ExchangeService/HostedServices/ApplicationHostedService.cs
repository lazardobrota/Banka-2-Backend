using Bank.ExchangeService.BackgroundServices;

namespace Bank.ExchangeService.HostedServices;

public class ApplicationHostedService(IHostApplicationLifetime applicationLifetime, DatabaseBackgroundService databaseBackgroundService) : IHostedService
{
    private readonly IHostApplicationLifetime  m_ApplicationLifetime       = applicationLifetime;
    private readonly DatabaseBackgroundService m_DatabaseBackgroundService = databaseBackgroundService;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        m_ApplicationLifetime.ApplicationStarted.Register(() => { m_DatabaseBackgroundService.OnApplicationStarted(); });

        m_ApplicationLifetime.ApplicationStopped.Register(() => { m_DatabaseBackgroundService.OnApplicationStopped(); });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
