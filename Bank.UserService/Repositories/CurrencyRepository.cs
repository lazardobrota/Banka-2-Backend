using System.Linq.Expressions;

using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Bank.UserService.Repositories;

public interface ICurrencyRepository
{
    Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery, bool includeForeignEntity = true);

    Task<Currency?> FindById(Guid id, bool includeForeignEntity = true);

    Task<Currency?> FindByCode(string currencyCode, bool includeForeignEntity = true);

    Task<bool> Exists(Guid currencyId);
}

public class CurrencyRepository(ApplicationContext context, IDbContextFactory<ApplicationContext> contextFactory) : ICurrencyRepository
{
    private readonly ApplicationContext                    m_Context        = context;
    private readonly IDbContextFactory<ApplicationContext> m_ContextFactory = contextFactory;

    private Task<ApplicationContext> CreateContext => m_ContextFactory.CreateDbContextAsync();

    public async Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery, bool includeForeignEntity)
    {
        var currencyQuery = m_Context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = m_Context.Currencies.IncludeAll().AsQueryable();

        if (!string.IsNullOrEmpty(currencyFilterQuery.Name))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Name, $"%{currencyFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(currencyFilterQuery.Code))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Code, $"%{currencyFilterQuery.Code}%"));

        return await currencyQuery.ToListAsync();
    }

    public async Task<Currency?> FindById(Guid id, bool includeForeignEntity)
    {
        await using var context = await CreateContext;

        return await FindById(id, includeForeignEntity, context);
    }

    public async Task<Currency?> FindByCode(string currencyCode, bool includeForeignEntity)
    {
        await using var context = await CreateContext;

        return await FindByCode(currencyCode, includeForeignEntity, context);
    }

    public async Task<bool> Exists(Guid currencyId)
    {
        await using var context = await CreateContext;

        return await Exists(currencyId, context);
    }

    #region Static Repository Calls

    private static async Task<Currency?> FindById(Guid id, bool includeForeignEntity, ApplicationContext context)
    {
        var currencyQuery = context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = context.Currencies.IncludeAll()
                                   .AsQueryable();

        return await currencyQuery.Where(currency => currency.Id == id)
                                  .FirstOrDefaultAsync();
    }

    private static async Task<Currency?> FindByCode(string currencyCode, bool includeForeignEntity, ApplicationContext context)
    {
        var currencyQuery = context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = context.Currencies.IncludeAll()
                                   .AsQueryable();

        return await currencyQuery.Where(currency => EF.Functions.ILike(currency.Code, $"{currencyCode}"))
                                  .FirstOrDefaultAsync();
    }

    private static Task<bool> Exists(Guid currencyId, ApplicationContext context)
    {
        return context.Currencies.AnyAsync(currency => currency.Id == currencyId);
    }

    #endregion
}

public static partial class RepositoryExtensions
{
    public static IIncludableQueryable<Currency, object?> IncludeAll(this DbSet<Currency> set)
    {
        return set.Include(currency => currency.Countries)
                  .ThenIncludeAll(currency => currency.Countries, nameof(Country.Currency));
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, Currency?> value,
                                                                                 Expression<Func<TEntity, Currency?>>          navigationExpression, params string[] excludeProperties)
    where TEntity : class
    {
        IIncludableQueryable<TEntity, object?> query = value;

        if (!excludeProperties.Contains(nameof(Currency.Countries)))
            query = query.Include(navigationExpression)
                         .ThenInclude(currency => currency!.Countries);

        return query;
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, List<Currency>> value,
                                                                                 Expression<Func<TEntity, List<Currency>>> navigationExpression, params string[] excludeProperties)
    where TEntity : class
    {
        IIncludableQueryable<TEntity, object?> query = value;

        if (!excludeProperties.Contains(nameof(Currency.Countries)))
            query = query.Include(navigationExpression)
                         .ThenInclude(currency => currency.Countries);

        return query;
    }
}
