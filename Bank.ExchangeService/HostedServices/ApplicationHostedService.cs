using Bank.ExchangeService.BackgroundServices;
using Bank.ExchangeService.Database.Processors;

namespace Bank.ExchangeService.HostedServices;

public class ApplicationHostedService(DatabaseBackgroundService databaseBackgroundService, IEnumerable<IRealtimeProcessor> realtimeProcessors) : IHostedService
{
    private readonly DatabaseBackgroundService       m_DatabaseBackgroundService = databaseBackgroundService;
    private readonly IEnumerable<IRealtimeProcessor> m_RealtimeProcessors        = realtimeProcessors;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        m_DatabaseBackgroundService.OnApplicationStarted();

        await Task.WhenAll(m_RealtimeProcessors.Select(realtimeProcessor => realtimeProcessor.OnApplicationStarted(cancellationToken)));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        m_DatabaseBackgroundService.OnApplicationStopped();

        await Task.WhenAll(m_RealtimeProcessors.Select(realtimeProcessor => realtimeProcessor.OnApplicationStopped(cancellationToken)));
    }
}
