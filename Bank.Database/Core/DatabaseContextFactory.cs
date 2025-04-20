using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

namespace Bank.Database.Core;

public interface IDatabaseContextFactory<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    Task<TDatabaseContext> CreateContext { get; }
}

#region Ado.NET Pooling

internal abstract class AbstractDefaultContextFactory<TDatabaseContext>(IDefaultContextPool contextPool)
: IDatabaseContextFactory<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    internal readonly IDefaultContextPool ContextPool = contextPool;

    public abstract Task<TDatabaseContext> CreateContext { get; }
}

internal class PostgresDefaultContextFactory<TDatabaseContext>(IDefaultContextPool contextPool, IDbContextFactory<TDatabaseContext> contextFactory)
: AbstractDefaultContextFactory<TDatabaseContext>(contextPool) where TDatabaseContext : DatabaseContext
{
    private readonly IDbContextFactory<TDatabaseContext> m_ContextFactory = contextFactory;

    public override Task<TDatabaseContext> CreateContext => GetOrCreateContext();

    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private async Task<TDatabaseContext> GetOrCreateContext()
    {
        await ContextPool.Semaphore.WaitAsync();

        var context = await m_ContextFactory.CreateDbContextAsync();

        context.DisposeAfterAction = () => ContextPool.Semaphore.Release();

        return context;
    }
}

#endregion

[Obsolete("Manual Connection Pooling", true)]
internal abstract class AbstractContextFactory<TDatabaseContext> : IDatabaseContextFactory<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    internal readonly IDatabaseContextPool<TDatabaseContext> ContextPool;

    protected AbstractContextFactory(IDatabaseContextPool<TDatabaseContext> contextPool)
    {
        ContextPool = contextPool;

        ContextPool.Initialise()
                   .Wait();
    }

    public abstract Task<TDatabaseContext> CreateContext { get; }
}

[Obsolete("Manual Connection Pooling", true)]
internal class PostgresDatabaseContextFactory<TDatabaseContext>(IDatabaseContextPool<TDatabaseContext> contextPool, IDbContextFactory<TDatabaseContext> contextFactory)
: AbstractContextFactory<TDatabaseContext>(contextPool) where TDatabaseContext : DatabaseContext
{
    private readonly IDbContextFactory<TDatabaseContext> m_ContextFactory = contextFactory;

    public override Task<TDatabaseContext> CreateContext => GetOrCreateContext();

    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private async Task<TDatabaseContext> GetOrCreateContext()
    {
        await ContextPool.Semaphore.WaitAsync();

        if (ContextPool.OpenedConnectionQueue.TryDequeue(out var openedContext))
            return openedContext;

        if (ContextPool.ClosedConnectionQueue.TryDequeue(out var closedContext))
        {
            await closedContext.Database.OpenConnectionAsync();

            return closedContext;
        }

        var context = await m_ContextFactory.CreateDbContextAsync();

        // context.DisposeAction      = contextParam => DisposeContext((TDatabaseContext)contextParam);
        // context.DisposeActionAsync = contextParam => DisposeContextAsync((TDatabaseContext)contextParam);

        return context;
    }

    // [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    // public void DisposeContext(TDatabaseContext context)
    // {
    //     lock (ContextPool.Lock)
    //     {
    //         if (ContextPool.OpenedConnectionQueue.Count < ContextPool.MinConnections)
    //             ContextPool.OpenedConnectionQueue.Enqueue(context);
    //         else
    //         {
    //             context.Database.CloseConnection();
    //             ContextPool.ClosedConnectionQueue.Enqueue(context);
    //         }
    //
    //         ContextPool.Semaphore.Release();
    //     }
    // }
    //
    // [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    // public async ValueTask DisposeContextAsync(TDatabaseContext context)
    // {
    //     Task? closeConnectionTask = null;
    //
    //     lock (ContextPool.Lock)
    //     {
    //         if (ContextPool.OpenedConnectionQueue.Count < ContextPool.MinConnections)
    //             ContextPool.OpenedConnectionQueue.Enqueue(context);
    //         else
    //         {
    //             closeConnectionTask = context.Database.CloseConnectionAsync();
    //             ContextPool.ClosedConnectionQueue.Enqueue(context);
    //         }
    //     }
    //
    //     if (closeConnectionTask != null)
    //         await closeConnectionTask;
    //
    //     lock (ContextPool.Lock)
    //         ContextPool.Semaphore.Release();
    // }
}
