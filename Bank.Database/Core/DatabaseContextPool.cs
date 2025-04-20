using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Bank.Database.Configurations;

using Microsoft.EntityFrameworkCore;

using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Bank.Database.Core;

#region Ado.NET Pooling

public interface IDefaultContextPool
{
    SemaphoreSlim Semaphore      { get; }
    int           MinConnections { get; }
    int           MaxConnections { get; }
}

internal class PostgresDefaultContextPool : IDefaultContextPool
{
    public SemaphoreSlim Semaphore { private set; get; } = null!;

    public const int ConnectionExcess = 10;
    public       int MinConnections { private set; get; } = Configuration.Database.MinConnections;
    public       int MaxConnections { private set; get; } = Configuration.Database.MaxConnections;

    public PostgresDefaultContextPool()
    {
        Initialise()
        .Wait();
    }

    [SuppressMessage("Usage",     "EF1001:Internal EF Core API usage.")]
    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private async Task Initialise()
    {
        if (MaxConnections == 0)
        {
            await using var connection = new NpgsqlConnection(Configuration.Database.GetConnectionString());
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("SHOW max_connections", connection);

            MaxConnections = int.Parse((string)(await command.ExecuteScalarAsync())!) - ConnectionExcess;
        }

        if (MinConnections == 0)
            MinConnections = MaxConnections / 2;

        Semaphore = new SemaphoreSlim(MaxConnections);
    }
}

#endregion

[Obsolete("Manual Connection Pooling", true)]
public interface IDatabaseContextPool<TDatabaseContext> where TDatabaseContext : DbContext
{
    Task Initialise();

    ConcurrentQueue<TDatabaseContext> OpenedConnectionQueue { get; }
    ConcurrentQueue<TDatabaseContext> ClosedConnectionQueue { get; }
    SemaphoreSlim                     Semaphore             { get; }
    Lock                              Lock                  { get; }
    int                               MinConnections        { get; }
    int                               MaxConnections        { get; }
}

[Obsolete("Manual Connection Pooling", true)]
internal class PostgresDatabaseContextPool<TDatabaseContext>(DbContextOptions<TDatabaseContext> contextOptions)
: IDatabaseContextPool<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    public readonly DbContextOptions<TDatabaseContext> ContextOptions = contextOptions;

    public ConcurrentQueue<TDatabaseContext> OpenedConnectionQueue { get; } = new();
    public ConcurrentQueue<TDatabaseContext> ClosedConnectionQueue { get; } = new();

    public Lock          Lock      { get; }              = new();
    public SemaphoreSlim Semaphore { private set; get; } = null!;

    public int MinConnections { private set; get; }
    public int MaxConnections { private set; get; }

    private bool m_Initialised = false;

    [SuppressMessage("Usage",     "EF1001:Internal EF Core API usage.")]
    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    public async Task Initialise()
    {
        if (m_Initialised)
            return;

        lock (Lock)
        {
            if (m_Initialised)
                return;

            m_Initialised = true;
        }

        await using var connection = new NpgsqlConnection(ContextOptions.FindExtension<NpgsqlOptionsExtension>()!.ConnectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand("SHOW max_connections", connection);

        MaxConnections = int.Parse((string)(await command.ExecuteScalarAsync())!) - 10;
        MinConnections = MaxConnections / 2;

        Semaphore = new SemaphoreSlim(MaxConnections);
    }
}
