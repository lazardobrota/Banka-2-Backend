using Bank.UserService.Configurations;
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
        if (Configuration.Database.CreateDrop)
            Context.Database.EnsureDeletedAsync()
                   .Wait();

        Context.Database.EnsureCreatedAsync()
               .Wait();

        Context.SeedUsers()
               .Wait();

        Context.SeedAccounts()
               .Wait();

        Context.SeedCardTypes()
               .Wait();

        Context.SeedCards()
               .Wait();

        Context.SeedCurrency()
               .Wait();

        Context.SeedCountry()
               .Wait();

        Context.SeedCompany()
               .Wait();
    }

    public void OnApplicationStopped() { }
}
