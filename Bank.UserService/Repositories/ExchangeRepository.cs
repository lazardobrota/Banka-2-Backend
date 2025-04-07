using Bank.Application.Queries;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface IExchangeRepository
{
    public Task<List<Exchange>> FindAll(ExchangeFilterQuery exchangeFilterQuery);

    public Task<Exchange?> FindById(Guid id);

    public Task<Exchange?> FindByCurrencyFromAndCurrencyTo(Guid firstCurrencyId, Guid secondCurrencyId);

    public Task<Exchange?> FindByCurrencyFromAndCurrencyTo(Currency firstCurrency, Currency secondCurrency);

    public Task<Exchange> Add(Exchange exchange);

    public Task<Exchange> Update(Exchange exchange);
}

public class ExchangeRepository(ApplicationContext context) : IExchangeRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<List<Exchange>> FindAll(ExchangeFilterQuery exchangeFilterQuery)
    {
        var exchangeQueue = m_Context.Exchanges.Include(exchange => exchange.CurrencyFrom)
                                     .Include(exchange => exchange.CurrencyTo)
                                     .AsQueryable();

        if (!string.IsNullOrEmpty(exchangeFilterQuery.CurrencyCode))
            exchangeQueue = exchangeQueue.Where(exchange => EF.Functions.ILike(exchange.CurrencyFrom!.Code.ToLower(), $"%{exchangeFilterQuery.CurrencyCode.ToLower()}%") ||
                                                            EF.Functions.ILike(exchange.CurrencyTo!.Code.ToLower(),   $"%{exchangeFilterQuery.CurrencyCode.ToLower()}%"));

        if (exchangeFilterQuery.CurrencyId != Guid.Empty)
            exchangeQueue = exchangeQueue.Where(exchange => exchange.CurrencyFromId == exchangeFilterQuery.CurrencyId || exchange.CurrencyToId == exchangeFilterQuery.CurrencyId);

        if (exchangeFilterQuery.Date == DateOnly.MinValue)
            exchangeFilterQuery.Date = DateOnly.FromDateTime(DateTime.UtcNow);

        exchangeQueue = exchangeQueue.Where(exchange => exchangeFilterQuery.Date                  <= DateOnly.FromDateTime(exchange.CreatedAt) &&
                                                        DateOnly.FromDateTime(exchange.CreatedAt) < exchangeFilterQuery.Date.AddDays(1));

        return await exchangeQueue.ToListAsync();
    }

    public async Task<Exchange?> FindById(Guid id)
    {
        return await m_Context.Exchanges.Include(exchange => exchange.CurrencyFrom)
                              .Include(exchange => exchange.CurrencyTo)
                              .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Exchange?> FindByCurrencyFromAndCurrencyTo(Guid firstCurrencyId, Guid secondCurrencyId)
    {
        return await m_Context.Exchanges.Include(exchange => exchange.CurrencyFrom)
                              .Include(exchange => exchange.CurrencyTo)
                              .OrderByDescending(exchange => exchange.CreatedAt)
                              .FirstOrDefaultAsync(exchange => (exchange.CurrencyFromId == firstCurrencyId  && exchange.CurrencyToId == secondCurrencyId) ||
                                                               (exchange.CurrencyFromId == secondCurrencyId && exchange.CurrencyToId == firstCurrencyId));
    }

    public async Task<Exchange?> FindByCurrencyFromAndCurrencyTo(Currency firstCurrency, Currency secondCurrency)
    {
        return await FindByCurrencyFromAndCurrencyTo(firstCurrency.Id, secondCurrency.Id);
    }

    public async Task<Exchange> Add(Exchange exchange)
    {
        var addExchange = await m_Context.Exchanges.AddAsync(exchange);

        await m_Context.SaveChangesAsync();

        return addExchange.Entity;
    }

    public async Task<Exchange> Update(Exchange exchange)
    {
        await m_Context.Exchanges.Where(dbExchange => dbExchange.Id == exchange.Id)
                       .ExecuteUpdateAsync(setter => setter.SetProperty(dbExchange => dbExchange.Commission, exchange.Commission));

        return exchange;
    }
}
