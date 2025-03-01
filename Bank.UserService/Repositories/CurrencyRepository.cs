using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ICurrencyRepository
{
    Task<Page<Currency>> FindAll(Pageable pageable);

    Task<Currency?> FindById(Guid id);

    Task<Currency> Add(Currency currency);

    Task<Currency> Update(Currency oldCurrency, Currency currency);
}

public class CurrencyRepository(ApplicationContext context) : ICurrencyRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<Page<Currency>> FindAll(Pageable pageable)
    {
        var currencyQuery = m_Context.Currencies.AsQueryable();

        int totalElements = await currencyQuery.CountAsync();

        var currencies = await currencyQuery.Skip((pageable.Page - 1) * pageable.Size)
                                            .Take(pageable.Size)
                                            .ToListAsync();

        return new Page<Currency>(currencies, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Currency?> FindById(Guid id)
    {
        return await m_Context.Currencies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Currency> Add(Currency currency)
    {
        var addedCurrency = await m_Context.Currencies.AddAsync(currency);

        await m_Context.SaveChangesAsync();

        return addedCurrency.Entity;
    }

    public async Task<Currency> Update(Currency oldCurrency, Currency currency)
    {
        m_Context.Currencies.Entry(oldCurrency)
                 .State = EntityState.Detached;

        var updatedCurrency = m_Context.Currencies.Update(currency);

        await m_Context.SaveChangesAsync();

        return updatedCurrency.Entity;
    }
}
