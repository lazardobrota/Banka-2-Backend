using Bank.Application.Endpoints;
using Bank.Application.Extensions;
using Bank.LoanService.Database.Seeders;
using Bank.UserService.Application;
using Bank.UserService.Configurations;
using Bank.UserService.Database;
using Bank.UserService.Database.Seeders;
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
        var path = Directory.GetCurrentDirectory()
                            .GetParent(3);

        var beforeEnv = Configuration.Database.GetConnectionString();
        Env.Load(Directory.GetCurrentDirectory().GetParent(3));
        var afterEnv = Configuration.Database.GetConnectionString();

        var services = new ServiceCollection();

        services.AddServices();
        services.AddHttpServices();
        services.AddDatabase();
        services.AddHostedServices();

        var serviceProvider = services.BuildServiceProvider();

        var context = serviceProvider.CreateScope()
                                     .ServiceProvider.GetRequiredService<ApplicationContext>();

        SeedDatabase(context);

        return services;
    }

    public static void SeedDatabase(ApplicationContext context)
    {
        if (Configuration.Database.CreateDrop)
        {
            context.Database.EnsureDeletedAsync()
                   .Wait();
        }

        context.Database.EnsureCreatedAsync()
               .Wait();

        context.Database.EnsureCreatedAsync()
               .Wait();

        context.SeedClient()
               .Wait();

        context.SeedEmployee()
               .Wait();

        context.SeedCurrency()
               .Wait();

        context.SeedCountry()
               .Wait();

        context.SeedCompany()
               .Wait();

        context.SeedAccountType()
               .Wait();

        context.SeedAccount()
               .Wait();

        context.SeedLoanTypes()
               .Wait();

        context.SeedLoans()
               .Wait();

        context.SeedInstallments()
               .Wait();

        context.SeedAccountCurrency()
               .Wait();

        context.SeedCadType()
               .Wait();

        context.SeedCard()
               .Wait();

        context.SeedTransactionCode()
               .Wait();

        context.SeedExchangeHardcoded()
               .Wait();
    }
}
