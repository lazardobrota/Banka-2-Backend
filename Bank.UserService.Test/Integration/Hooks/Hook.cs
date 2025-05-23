using Bank.Application.Extensions;
using Bank.Database;
using Bank.Permissions.Services;
using Bank.UserService.Application;
using Bank.UserService.BackgroundServices;
using Bank.UserService.Database;
using Bank.UserService.Test.Integration.Services;

using DotNetEnv;

using Microsoft.Extensions.DependencyInjection;

using Reqnroll.BoDi;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Bank.UserService.Test.Hooks;

[Binding]
public class Hooks
{
    [BeforeTestRun]
    public static void IncreaseResolutionTimeout()
    {
        ObjectContainer.DefaultConcurrentObjectResolutionTimeout = TimeSpan.FromSeconds(10);
    }

    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        Env.Load(Directory.GetCurrentDirectory()
                          .UpDirectory(3));

        var services = new ServiceCollection();

        services.AddSingleton<IAuthorizationServiceFactory, TestAuthorizationServiceFactory>();
        services.AddServices();
        services.AddBackgroundServices();
        services.AddHttpServices();
        services.AddDatabaseServices<ApplicationContext>();
        services.AddHostedServices();
        services.AddOpenApiExamples();

        var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<DatabaseBackgroundService>()
                       .OnApplicationStarted(CancellationToken.None)
                       .Wait();

        serviceProvider.GetRequiredService<TransactionBackgroundService>()
                       .OnApplicationStarted()
                       .Wait();

        return services;
    }
}
