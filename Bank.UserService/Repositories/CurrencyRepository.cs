using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ICurrencyRepository
{
    Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery, bool includeForeignEntity = true);

    Task<Currency?> FindById(Guid id, bool includeForeignEntity = true);

    Task<Currency?> FindByCode(string currencyCode, bool includeForeignEntity = true);
}

public class CurrencyRepository(ApplicationContext context) : ICurrencyRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<List<Currency>> FindAll(CurrencyFilterQuery currencyFilterQuery, bool includeForeignEntity)
    {
        var currencyQuery = m_Context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = currencyQuery.Include(c => c.Countries);

        if (!string.IsNullOrEmpty(currencyFilterQuery.Name))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Name, $"%{currencyFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(currencyFilterQuery.Code))
            currencyQuery = currencyQuery.Where(currency => EF.Functions.ILike(currency.Code, $"%{currencyFilterQuery.Code}%"));

        return await currencyQuery.ToListAsync();
    }

    public async Task<Currency?> FindById(Guid id, bool includeForeignEntity)
    {
        var currencyQuery = m_Context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = currencyQuery.Include(c => c.Countries);

        return await currencyQuery.Where(currency => currency.Id == id)
                                  .FirstOrDefaultAsync();
    }

    public async Task<Currency?> FindByCode(string currencyCode, bool includeForeignEntity)
    {
        var currencyQuery = m_Context.Currencies.AsQueryable();

        if (includeForeignEntity)
            currencyQuery = currencyQuery.Include(c => c.Countries);

        return await currencyQuery.Where(currency => EF.Functions.ILike(currency.Code, $"{currencyCode}"))
                                  .FirstOrDefaultAsync();
    }
}
