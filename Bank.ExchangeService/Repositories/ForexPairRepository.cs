using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface IForexPairRepository
{
    Task<Page<ForexPair>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<ForexPair?> FindById(Guid id);

    Task<ForexPair?> Create(ForexPair forexPair);
}

public class ForexPairRepository(DatabaseContext context) : IForexPairRepository
{
    private DatabaseContext m_DatabaseContext = context;

    public async Task<Page<ForexPair>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var query = m_DatabaseContext.ForexPairs.Include(forexPair => forexPair.StockExchange)
                                     .Include(forexPair => forexPair.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                                     .AsQueryable();

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            query = query.Where(forexPair => EF.Functions.ILike(forexPair.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            query = query.Where(forexPair => EF.Functions.ILike(forexPair.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            query = query.Where(forexPair => forexPair.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<ForexPair>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<ForexPair?> FindById(Guid id)
    {
        return await m_DatabaseContext.ForexPairs.Include(forexPair => forexPair.StockExchange)
                                      .Include(forexPair => forexPair.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                                      .FirstAsync(forexPair => forexPair.Id == id);
    }

    public async Task<ForexPair?> Create(ForexPair forexPair)
    {
        var added = await m_DatabaseContext.ForexPairs.AddAsync(forexPair);
        await m_DatabaseContext.SaveChangesAsync();
        return added.Entity;
    }
}
