using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ICurrencyRepository
{
    Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery);

    Task<Currency?> FindById(Guid id);

    Task<Currency?> FindByCode(string currencyCode);

    Task<bool> Exists(Guid currencyId);
}

public class CurrencyRepository(ApplicationContext context, IDbContextFactory<ApplicationContext> contextFactory) : ICurrencyRepository
{
    private readonly ApplicationContext                    m_Context        = context;
    private readonly IDbContextFactory<ApplicationContext> m_ContextFactory = contextFactory;

    private Task<ApplicationContext> CreateContext => m_ContextFactory.CreateDbContextAsync();
    
    public async Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery)
    {
        var currencyQuery = m_Context.Currencies.Include(c => c.Countries)
                                     .AsQueryable();

        if (!string.IsNullOrEmpty(currencyFilterQuery.Name))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Name, $"%{currencyFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(currencyFilterQuery.Code))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Code, $"%{currencyFilterQuery.Code}%"));

        return await currencyQuery.ToListAsync();
    }

    public async Task<Currency?> FindById(Guid id)
    {
        await using var context = await CreateContext;

        return await FindById(id, context);
    }

    public async Task<Currency?> FindByCode(string currencyCode)
    {
        await using var context = await CreateContext;

        return await FindByCode(currencyCode, context);
    }

    public async Task<bool> Exists(Guid currencyId)
    {
        await using var context = await CreateContext;

        return await Exists(currencyId, context);
    }

    #region Static Repository Calls
    
    private static async Task<Currency?> FindById(Guid id, ApplicationContext context)
    {
        return await context.Currencies.Include(currency => currency.Countries)
                            .Where(currency => currency.Id == id)
                            .FirstOrDefaultAsync();
    }

    private static async Task<Currency?> FindByCode(string currencyCode, ApplicationContext context)
    {
        return await context.Currencies.Include(currency => currency.Countries)
                            .Where(currency => EF.Functions.ILike(currency.Code, $"{currencyCode}"))
                            .FirstOrDefaultAsync();
    }

    private static Task<bool> Exists(Guid currencyId, ApplicationContext context)
    {
        return context.Currencies.AnyAsync(currency => currency.Id == currencyId);
    }
    
    #endregion
}
