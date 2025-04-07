using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface IStockRepository
{
    Task<List<Stock>> FindAll();

    Task<Page<Stock>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable);

    Task<Stock?> FindById(Guid id, QuoteFilterIntervalQuery filter);

    Task<Stock?> Create(Stock stock);
}

public class StockRepository(DatabaseContext context, IDbContextFactory<DatabaseContext> contextFactory) : IStockRepository
{
    private readonly DatabaseContext                    m_Context        = context;
    private readonly IDbContextFactory<DatabaseContext> m_ContextFactory = contextFactory;

    private Task<DatabaseContext> CreateContext => m_ContextFactory.CreateDbContextAsync();

    public async Task<List<Stock>> FindAll()
    {
        return await m_Context.Stocks.Include(stock => stock.StockExchange)
                              .ToListAsync();
    }

    public async Task<Page<Stock>> FindAll(QuoteFilterQuery quoteFilterQuery, Pageable pageable)
    {
        var stockQuery = m_Context.Stocks.Include(stock => stock.StockExchange)
                                  .AsQueryable();

        var fromDate = DateOnly.FromDateTime(quoteFilterQuery.Interval switch
                                             {
                                                 QuoteIntervalType.Week        => DateTime.Today.AddDays(-7),
                                                 QuoteIntervalType.Month       => DateTime.Today.AddMonths(-1),
                                                 QuoteIntervalType.ThreeMonths => DateTime.Today.AddMonths(-3),
                                                 QuoteIntervalType.Year        => DateTime.Today.AddYears(-1),
                                                 QuoteIntervalType.Max         => DateTime.MinValue,
                                                 _                             => DateTime.Today
                                             });

        stockQuery = stockQuery.Include(stock => stock.SortedQuotes.Where(quote => fromDate                               <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                                                   DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                                      .OrderByDescending(quote => quote.CreatedAt));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            stockQuery = stockQuery.Where(stock => EF.Functions.ILike(stock.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            stockQuery = stockQuery.Where(stock => EF.Functions.ILike(stock.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            stockQuery = stockQuery.Where(stock => stock.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await stockQuery.CountAsync();

        var items = await stockQuery.Skip((pageable.Page - 1) * pageable.Size)
                                    .Take(pageable.Size)
                                    .ToListAsync();

        return new Page<Stock>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<Stock?> FindById(Guid id, QuoteFilterIntervalQuery filter)
    {
        var fromDate = DateOnly.FromDateTime(filter.Interval switch
                                             {
                                                 QuoteIntervalType.Week        => DateTime.Today.AddDays(-7),
                                                 QuoteIntervalType.Month       => DateTime.Today.AddMonths(-1),
                                                 QuoteIntervalType.ThreeMonths => DateTime.Today.AddMonths(-3),
                                                 QuoteIntervalType.Year        => DateTime.Today.AddYears(-1),
                                                 QuoteIntervalType.Max         => DateTime.MinValue,
                                                 _                             => DateTime.Today
                                             });

        return await m_Context.Stocks.Include(stock => stock.StockExchange)
                              .Include(stock => stock.SortedQuotes.Where(quote => fromDate                               <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                                                  DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                                     .OrderByDescending(quote => quote.CreatedAt))
                              .FirstOrDefaultAsync(stock => stock.Id == id);
    }

    public async Task<Stock?> Create(Stock stock)
    {
        var added = await m_Context.Stocks.AddAsync(stock);
        await m_Context.SaveChangesAsync();
        return added.Entity;
    }
}
