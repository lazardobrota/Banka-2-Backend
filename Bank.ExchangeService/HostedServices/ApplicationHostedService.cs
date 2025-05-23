using Bank.ExchangeService.BackgroundServices;

namespace Bank.ExchangeService.HostedServices;

public class ApplicationHostedService(
    IHostApplicationLifetime  applicationLifetime,
    DatabaseBackgroundService databaseBackgroundService,
    OrderBackgroundService    orderBackgroundService
) : IHostedService
{
    private readonly IHostApplicationLifetime  m_ApplicationLifetime       = applicationLifetime;
    private readonly DatabaseBackgroundService m_DatabaseBackgroundService = databaseBackgroundService;
    private readonly OrderBackgroundService    m_OrderBackgroundService    = orderBackgroundService;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await m_OrderBackgroundService.OnApplicationStarted();

        m_ApplicationLifetime.ApplicationStarted.Register(() => { m_DatabaseBackgroundService.OnApplicationStarted(); });

        m_ApplicationLifetime.ApplicationStopped.Register(() => { m_DatabaseBackgroundService.OnApplicationStopped(); });

        // return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
