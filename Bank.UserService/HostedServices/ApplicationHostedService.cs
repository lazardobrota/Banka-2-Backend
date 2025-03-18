namespace Bank.UserService.HostedServices;

public class ApplicationHostedService(
    IHostApplicationLifetime applicationLifetime,
    DatabaseHostedService    databaseHostedService,
    ExchangeHostedService    exchangeHostedService,
    LoanHostedService        loanHostedService
) : IHostedService
{
    private readonly IHostApplicationLifetime m_ApplicationLifetime   = applicationLifetime;
    private readonly DatabaseHostedService    m_DatabaseHostedService = databaseHostedService;
    private readonly ExchangeHostedService    m_ExchangeHostedService = exchangeHostedService;
    private readonly LoanHostedService        m_LoanHostedService     = loanHostedService;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        m_ApplicationLifetime.ApplicationStarted.Register(m_DatabaseHostedService.OnApplicationStarted);
        m_ApplicationLifetime.ApplicationStarted.Register(m_ExchangeHostedService.OnApplicationStarted);
        m_ApplicationLifetime.ApplicationStarted.Register(m_LoanHostedService.OnApplicationStarted);

        m_ApplicationLifetime.ApplicationStarted.Register(m_ExchangeHostedService.OnApplicationStopped);
        m_ApplicationLifetime.ApplicationStopped.Register(m_DatabaseHostedService.OnApplicationStopped);
        m_ApplicationLifetime.ApplicationStopped.Register(m_LoanHostedService.OnApplicationStopped);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
