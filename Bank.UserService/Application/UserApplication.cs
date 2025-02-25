﻿using Bank.UserService.Database;
using Bank.UserService.HostedServices;
using Bank.UserService.Repositories;
using Bank.UserService.Services;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Application;

public class UserApplication
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddServiceApplication();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceApplication(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IUserService, Services.UserService>();
        services.AddTransient<IClientService, ClientService>();

        services.AddSingleton<DatabaseHostedService>();
        Console.WriteLine(DatabaseConfig.GetConnectionString());

        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(DatabaseConfig.GetConnectionString()), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        services.AddHostedService<ApplicationHostedService>();

        return services;
    }
}
