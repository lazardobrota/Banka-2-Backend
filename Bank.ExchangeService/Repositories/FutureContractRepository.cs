using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface IFutureContractRepository
{
    Task<Page<FutureContract>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<FutureContract?> FindById(Guid id);

    Task<FutureContract?> Create(FutureContract futureContract);
}

public class FutureContractRepository(DatabaseContext context) : IFutureContractRepository
{
    private DatabaseContext m_Context = context;

    public async Task<Page<FutureContract>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var query = m_Context.FutureContracts.Include(futureContract => futureContract.StockExchange)
                             .Include(futureContract => futureContract.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                             .AsQueryable();

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            query = query.Where(futureContract => EF.Functions.ILike(futureContract.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            query = query.Where(futureContract => EF.Functions.ILike(futureContract.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            query = query.Where(futureContract => futureContract.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<FutureContract>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<FutureContract?> FindById(Guid id)
    {
        return await m_Context.FutureContracts.Include(futureContract => futureContract.StockExchange)
                              .Include(futureContract => futureContract.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                              .FirstAsync(futureContract => futureContract.Id == id);
    }

    public async Task<FutureContract?> Create(FutureContract futureContract)
    {
        var added = await m_Context.FutureContracts.AddAsync(futureContract);
        await m_Context.SaveChangesAsync();
        return added.Entity;
    }
}
