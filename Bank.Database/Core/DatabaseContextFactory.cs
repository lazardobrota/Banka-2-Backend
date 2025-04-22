using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

namespace Bank.Database.Core;

public interface IDatabaseContextFactory<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    Task<TDatabaseContext> CreateContext            { get; }
    Task<TDatabaseContext> CreateDistributedContext { get; }
}

#region Ado.NET Pooling

internal abstract class AbstractDefaultContextFactory<TDatabaseContext>(IDefaultContextPool contextPool)
: IDatabaseContextFactory<TDatabaseContext> where TDatabaseContext : DatabaseContext
{
    internal readonly IDefaultContextPool ContextPool = contextPool;

    public abstract Task<TDatabaseContext> CreateContext            { get; }
    public abstract Task<TDatabaseContext> CreateDistributedContext { get; }
}

internal class PostgresDefaultContextFactory<TDatabaseContext>(IDefaultContextPool contextPool, IDbContextFactory<TDatabaseContext> contextFactory)
: AbstractDefaultContextFactory<TDatabaseContext>(contextPool) where TDatabaseContext : DatabaseContext
{
    private readonly IDbContextFactory<TDatabaseContext> m_ContextFactory = contextFactory;

    private readonly SemaphoreSlim m_SemaphoreDefault     = new(contextPool.MaxConnections);
    private readonly SemaphoreSlim m_SemaphoreDistributed = new(contextPool.MaxConnections / 5);

    public override Task<TDatabaseContext> CreateContext            => GetOrCreateContext();
    public override Task<TDatabaseContext> CreateDistributedContext => GetOrCreateDistributedContext();

    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private async Task<TDatabaseContext> GetOrCreateContext()
    {
        await m_SemaphoreDefault.WaitAsync();

        var context = await m_ContextFactory.CreateDbContextAsync();

        context.DisposeAfterAction = () => m_SemaphoreDefault.Release();

        return context;
    }

    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private async Task<TDatabaseContext> GetOrCreateDistributedContext()
    {
        await m_SemaphoreDistributed.WaitAsync();
        await m_SemaphoreDefault.WaitAsync();
        
        var context = await m_ContextFactory.CreateDbContextAsync();

        context.DisposeAfterAction = () =>
                                     {
                                         m_SemaphoreDefault.Release();
                                         m_SemaphoreDistributed.Release();
                                     };

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

    public abstract Task<TDatabaseContext> CreateContext            { get; }
    public abstract Task<TDatabaseContext> CreateDistributedContext { get; }
}

[Obsolete("Manual Connection Pooling", true)]
internal class PostgresDatabaseContextFactory<TDatabaseContext>(IDatabaseContextPool<TDatabaseContext> contextPool, IDbContextFactory<TDatabaseContext> contextFactory)
: AbstractContextFactory<TDatabaseContext>(contextPool) where TDatabaseContext : DatabaseContext
{
    private readonly IDbContextFactory<TDatabaseContext> m_ContextFactory = contextFactory;

    public override Task<TDatabaseContext> CreateContext            => GetOrCreateContext();
    public override Task<TDatabaseContext> CreateDistributedContext { get; } = null!;

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
