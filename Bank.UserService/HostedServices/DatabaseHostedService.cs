using Bank.UserService.Database;

namespace Bank.UserService.HostedServices;

public class DatabaseHostedService(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider m_ServiceProvider = serviceProvider;

    private ApplicationContext Context =>
    m_ServiceProvider.CreateScope()
                     .ServiceProvider.GetRequiredService<ApplicationContext>();

    public void OnApplicationStarted()
    {
        Context.Database.EnsureCreatedAsync()
               .Wait();

        Context.SeedUsers()
               .Wait();

        Context.SeedAccounts()
               .Wait();
    }

    public void OnApplicationStopped() { }
}
