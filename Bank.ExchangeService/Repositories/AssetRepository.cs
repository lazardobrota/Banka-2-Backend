using System.Linq.Expressions;

using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.Database.Core;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;
using Bank.Permissions.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using DatabaseContext = Bank.ExchangeService.Database.DatabaseContext;

namespace Bank.ExchangeService.Repositories;

public interface IAssetRepository
{
    Task<Page<Asset>> FindAll(AssetFilterQuery filter, Pageable pageable);

    Task<Asset?> FindById(Guid id);

    Task<bool> Add(Asset asset);

    Task<bool> Remove(Asset asset);
}

public class AssetRepository(IAuthorizationService authorizationService, IDatabaseContextFactory<DatabaseContext> contextFactory) : IAssetRepository
{
    private readonly IDatabaseContextFactory<DatabaseContext> m_ContextFactory = contextFactory;

    private readonly IAuthorizationService m_AuthorizationService = authorizationService;

    public async Task<Page<Asset>> FindAll(AssetFilterQuery filter, Pageable pageable)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var assetQuery = context.Assets.IncludeAll()
                                .AsQueryable();

        if (m_AuthorizationService.Permissions == Permission.Client || m_AuthorizationService.Permissions == Permission.Employee)
            assetQuery = assetQuery.Where(asset => asset.ActuaryId == m_AuthorizationService.UserId);

        assetQuery = assetQuery.OrderByDescending(asset => asset.ModifiedAt);

        var assets = await assetQuery.Skip((pageable.Page - 1) * pageable.Size)
                                     .Take(pageable.Size)
                                     .ToListAsync();

        var totalElements = await assetQuery.CountAsync();

        return new Page<Asset>(assets, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Asset?> FindById(Guid id)
    {
        await using var context = await m_ContextFactory.CreateContext;

        return await context.Assets.IncludeAll()
                            .FirstOrDefaultAsync(asset => asset.Id == id);
    }

    public async Task<bool> Add(Asset asset)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var result = await context.Assets.Where(dbAsset => dbAsset.ActuaryId == asset.ActuaryId && dbAsset.SecurityId == asset.SecurityId)
                                  .ExecuteUpdateAsync(setters => setters.SetProperty(dbAsset => dbAsset.ModifiedAt, DateTime.UtcNow)
                                                                        .SetProperty(dbAsset => dbAsset.Quantity, dbAsset => dbAsset.Quantity + asset.Quantity)
                                                                        .SetProperty(dbAsset => dbAsset.AveragePrice,
                                                                                     dbAsset => dbAsset.AveragePrice + asset.Quantity *
                                                                                                (asset.AveragePrice - dbAsset.AveragePrice) / (asset.Quantity + dbAsset.Quantity)));

        if (result == 1)
            return true;

        await context.Assets.AddAsync(asset);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Remove(Asset asset)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var result = await context.Assets.Where(dbAsset => dbAsset.ActuaryId == asset.ActuaryId && dbAsset.SecurityId == asset.SecurityId)
                                  .ExecuteUpdateAsync(setters => setters.SetProperty(dbAsset => dbAsset.ModifiedAt, DateTime.UtcNow)
                                                                        .SetProperty(dbAsset => dbAsset.Quantity, dbAsset => dbAsset.Quantity - asset.Quantity));

        return result == 1;
    }
}

public static partial class RepositoryExtensions
{
    public static IIncludableQueryable<Asset, object?> IncludeAll(this DbSet<Asset> set)
    {
        return set.Include(asset => asset.Security);
        // .ThenIncludeAll(asset => asset.Security); TODO
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, Asset?> value,
                                                                                 Expression<Func<TEntity, Asset?>>          navigationExpression, params string[] excludeProperties)
    where TEntity : class
    {
        IIncludableQueryable<TEntity, object?> query = value;

        if (!excludeProperties.Contains(nameof(Asset.Security)))
            query = query.Include(navigationExpression)
                         .ThenInclude(asset => asset!.Security);

        return query;
    }

    public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, List<Asset>> value,
                                                                                 Expression<Func<TEntity, List<Asset>>> navigationExpression, params string[] excludeProperties)
    where TEntity : class
    {
        IIncludableQueryable<TEntity, object?> query = value;

        if (!excludeProperties.Contains(nameof(Asset.Security)))
            query = query.Include(navigationExpression)
                         .ThenInclude(asset => asset.Security);

        return query;
    }
}
