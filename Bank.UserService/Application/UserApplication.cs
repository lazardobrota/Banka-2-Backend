using Bank.UserService.Database;

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
        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(DatabaseConfig.GetConnectionString()), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        return services;
    }
}