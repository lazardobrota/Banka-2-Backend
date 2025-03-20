using Bank.Application.Endpoints;
using Bank.Application.Extensions;
using Bank.UserService.Application;
using Bank.UserService.HostedServices;
using Bank.UserService.Models;
using Bank.UserService.Repositories;
using Bank.UserService.Services;

using DotNetEnv;

using Microsoft.Extensions.DependencyInjection;

using Reqnroll.BoDi;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Bank.UserService.Test.Hooks;

[Binding]
public class Hooks
{
    private class DontSendEmailService : IEmailService
    {
        public Task<Result> Send(EmailType type, User user) => Task.FromResult(Result.Ok());
    }

    [BeforeTestRun]
    public static void IncreaseResolutionTimeout()
    {
        ObjectContainer.DefaultConcurrentObjectResolutionTimeout = TimeSpan.FromSeconds(10);
    }

    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        Env.Load(Directory.GetCurrentDirectory().UpDirectory(3));

        var services = new ServiceCollection();

        services.AddServices();
        services.AddHttpServices();
        services.AddDatabase();
        services.AddHostedServices();
        services.AddSwagger();

        var serviceProvider = services.BuildServiceProvider();
        
        serviceProvider.GetRequiredService<DatabaseHostedService>().OnApplicationStarted();

        return services;
    }
}
