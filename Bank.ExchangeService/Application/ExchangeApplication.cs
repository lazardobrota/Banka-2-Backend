using System.IdentityModel.Tokens.Jwt;

using Bank.Application;
using Bank.ExchangeService.BackgroundServices;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Database.WebSockets;
using Bank.ExchangeService.HostedServices;
using Bank.ExchangeService.Repositories;
using Bank.ExchangeService.Services;
using Bank.Http;
using Bank.OpenApi;
using Bank.Permissions;

using DotNetEnv;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Application;

public class ExchangeApplication
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Env.Load();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        builder.Services.AddSignalR();
        builder.Services.AddValidation();
        builder.Services.AddServices();
        builder.Services.AddDatabase();
        builder.Services.AddHostedServices();
        builder.Services.AddBackgroundServices();
        builder.Services.AddHttpServices();

        builder.Services.AddCors();
        builder.Services.AddAuthenticationServices();
        builder.Services.AddAuthorizationServices();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiServices();
        builder.Services.AddOpenApiExamples();

        var app = builder.Build();
        
        app.UseCors(Configuration.Policy.FrontendApplication);

        app.MapHub<SecurityHub>("security-hub");

        app.MapOpenApiServices();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IStockExchangeRepository, StockExchangeRepository>();
        services.AddScoped<IStockExchangeService, StockExchangeService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IOptionService, OptionService>();
        services.AddScoped<IForexPairService, ForexPairService>();
        services.AddScoped<IFutureContractService, FutureContractService>();
        services.AddScoped<IQuoteRepository, QuoteRepository>();
        services.AddScoped<ISecurityRepository, SecurityRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }

    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseBackgroundService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(Configuration.Database.GetConnectionString()), ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        services.AddDbContextFactory<DatabaseContext>(options => options.UseNpgsql(Configuration.Database.GetConnectionString()));

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ApplicationHostedService>();

        return services;
    }

    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddUserServiceHttpClient();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode  = CascadeMode.Stop;

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AssemblyInfo>();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy(Configuration.Policy.FrontendApplication, policy => policy.WithOrigins(Configuration.Frontend.BaseUrl)
                                                                                                                .AllowAnyHeader()
                                                                                                                .AllowAnyMethod()
                                                                                                                .AllowCredentials()));

        return services;
    }

    public static IServiceCollection AddOpenApiExamples(this IServiceCollection services)
    {
        return services;
    }
}
