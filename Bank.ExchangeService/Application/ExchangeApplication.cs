using System.IdentityModel.Tokens.Jwt;

using Bank.Application;
using Bank.ExchangeService.BackgroundServices;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Database.WebSockets;
using Bank.ExchangeService.HostedServices;
using Bank.ExchangeService.Repositories;
using Bank.ExchangeService.Services;
using Bank.Permissions;

using DotNetEnv;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        builder.Services.AddSwagger();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(Configuration.Policy.FrontendApplication);

        app.MapHub<SecurityHub>("security-hub");

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

        services.AddHttpClient(Configuration.HttpClient.Name.UserService, httpClient =>
                                                                          {
                                                                              httpClient.BaseAddress = new Uri($"{Configuration.HttpClient.BaseUrl.UserService}");
                                                                              //TODO
                                                                              //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjEzMDU5MzY1NzI1NCwiaWQiOiJjNmY0NDEzMy0wOGYyLTRhNDMtYmQ2NS05Y2ZiNmIxM2ZhNWIiLCJwZXJtaXNzaW9uIjoiMiIsInJvbGUiOiJBZG1pbiIsImlhdCI6MTc0NDYzODQzNCwibmJmIjoxNzQ0NjM4NDM0fQ.1cA329l2bWUlENwYrq03l1yQ0Jxw597kw-YUT0WipiI");
                                                                          });

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

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
                               {
                                   config.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeService", Version = "v1" });

                                   config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                          {
                                                                              Description = "Authorization: Bearer {token}",
                                                                              Name        = "Authorization",
                                                                              In          = ParameterLocation.Header,
                                                                              Type        = SecuritySchemeType.ApiKey,
                                                                              Scheme      = "Bearer"
                                                                          });

                                   config.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                 {
                                                                     {
                                                                         new OpenApiSecurityScheme
                                                                         {
                                                                             Reference = new OpenApiReference
                                                                                         {
                                                                                             Type =
                                                                                             ReferenceType
                                                                                             .SecurityScheme,
                                                                                             Id =
                                                                                             "Bearer"
                                                                                         }
                                                                         },
                                                                         []
                                                                     }
                                                                 });
                               });

        return services;
    }
}
