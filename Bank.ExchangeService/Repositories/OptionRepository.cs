using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface IOptionRepository
{
    Task<Page<Option>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Option?> FindById(Guid id);

    Task<Option?> Create(Option option);
}

public class OptionRepository(DatabaseContext context) : IOptionRepository
{
    private readonly DatabaseContext m_Context = context;

    public async Task<Page<Option>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var query = m_Context.Options.Include(option => option.StockExchange)
                             .Include(option => option.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                             .AsQueryable();

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            query = query.Where(option => EF.Functions.ILike(option.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            query = query.Where(option => EF.Functions.ILike(option.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            query = query.Where(option => option.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<Option>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<Option?> FindById(Guid id)
    {
        return await m_Context.Options.Include(option => option.StockExchange)
                              .Include(option => option.SortedQuotes.OrderByDescending(quote => quote.CreatedAt))
                              .FirstAsync(option => option.Id == id);
    }

    public async Task<Option?> Create(Option option)
    {
        var added = await m_Context.Options.AddAsync(option);
        await m_Context.SaveChangesAsync();
        return added.Entity;
    }
}
