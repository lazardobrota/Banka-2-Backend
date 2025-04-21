using System.Linq.Expressions;

using Bank.Application.Domain;
using Bank.Database.Core;
using Bank.UserService.Database;
using Bank.UserService.Models;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Bank.UserService.Repositories;

public interface ITransactionCodeRepository
{
    Task<Page<TransactionCode>> FindAll(Pageable pageable);

    Task<TransactionCode?> FindById(Guid id);

    Task<bool> AddRange(IEnumerable<TransactionCode> transactionCodes);

    Task<bool> Exists(Guid id);

    Task<bool> IsEmpty();
}

public class TransactionCodeRepository(IDatabaseContextFactory<ApplicationContext> contextFactory) : ITransactionCodeRepository
{
    private readonly IDatabaseContextFactory<ApplicationContext> m_ContextFactory = contextFactory;

    public async Task<Page<TransactionCode>> FindAll(Pageable pageable)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var transactionCodeQuery = context.TransactionCodes.AsQueryable();

        var transactionCodes = await transactionCodeQuery.Skip((pageable.Page - 1) * pageable.Size)
                                                         .Take(pageable.Size)
                                                         .ToListAsync();

        var totalElements = await transactionCodeQuery.CountAsync();

        return new Page<TransactionCode>(transactionCodes, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<TransactionCode?> FindById(Guid id)
    {
        await using var context = await m_ContextFactory.CreateContext;

        return await context.TransactionCodes.FirstOrDefaultAsync(transactionCode => transactionCode.Id == id);
    }

    public async Task<bool> AddRange(IEnumerable<TransactionCode> transactionCodes)
    {
        await using var context = await m_ContextFactory.CreateContext;

        await context.BulkInsertAsync(transactionCodes, config => config.BatchSize = 2000);

        return true;
    }

    public async Task<bool> Exists(Guid id)
    {
        await using var context = await m_ContextFactory.CreateContext;

        return await context.TransactionCodes.AnyAsync(transactionCode => transactionCode.Id == id);
    }

    public async Task<bool> IsEmpty()
    {
        await using var context = await m_ContextFactory.CreateContext;

        return await context.TransactionCodes.AnyAsync() is not true;
    }
}

public static partial class RepositoryExtensions
{
    [Obsolete("This method does not have implementation.", true)]
    public static IIncludableQueryable<TransactionCode, object?> IncludeAll(this DbSet<TransactionCode> set)
    {
        return set.Include(transactionCode => transactionCode);
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, TransactionCode?> value,
                                                                                 Expression<Func<TEntity, TransactionCode?>>          navigationExpression,
                                                                                 params string[]                                      excludeProperties) where TEntity : class
    {
        return value;
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, List<TransactionCode>> value,
                                                                                 Expression<Func<TEntity, List<TransactionCode>>>          navigationExpression,
                                                                                 params string[]                                           excludeProperties) where TEntity : class
    {
        return value;
    }
}
