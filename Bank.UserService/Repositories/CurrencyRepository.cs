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
}

public class CurrencyRepository(ApplicationContext context) : ICurrencyRepository
{
    private readonly ApplicationContext m_Context = context;

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
        return await m_Context.Currencies.Include(currency => currency.Countries)
                              .Where(currency => currency.Id == id)
                              .FirstOrDefaultAsync();
    }

    public async Task<Currency?> FindByCode(string currencyCode)
    {
        return await m_Context.Currencies.Include(currency => currency.Countries)
                              .Where(currency => EF.Functions.ILike(currency.Code, $"{currencyCode}"))
                              .FirstOrDefaultAsync();
    }
}
