using Bank.Database.Configurations;
using Bank.Database.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Database;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseServices<TDatabaseContext>(this IServiceCollection services)
    where TDatabaseContext : DatabaseContext
    {
        services.AddSingleton<IDefaultContextPool, PostgresDefaultContextPool>();
        services.AddSingleton<IDatabaseContextFactory<TDatabaseContext>, PostgresDefaultContextFactory<TDatabaseContext>>();
        
        services.AddDbContextFactory<TDatabaseContext>(options => options.UseNpgsql(Configuration.Database.GetConnectionString()));

        return services;
    }
}
