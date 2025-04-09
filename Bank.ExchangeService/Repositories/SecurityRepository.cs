using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface ISecurityRepository
{
    Task<List<Security>> FindAll(SecurityType securityType);

    Task<Page<Security>> FindAll(QuoteFilterQuery quoteFilterQuery, SecurityType securityType, Pageable pageable);

    Task<Security?> FindById(Guid id, QuoteFilterIntervalQuery filter);

    Task<Security?> Create(Security security);

    Task<bool> CreateSecurities(List<Security> securities);
}

public class SecurityRepository(DatabaseContext context, IDbContextFactory<DatabaseContext> contextFactory) : ISecurityRepository
{
    private readonly DatabaseContext                    m_Context        = context;
    private readonly IDbContextFactory<DatabaseContext> m_ContextFactory = contextFactory;

    private Task<DatabaseContext> CreateContext => m_ContextFactory.CreateDbContextAsync();

    public async Task<List<Security>> FindAll(SecurityType securityType)
    {
        return await m_Context.Securities.Include(security => security.StockExchange)
                              .Where(security => security.SecurityType == securityType)
                              .ToListAsync();
    }

    public async Task<Page<Security>> FindAll(QuoteFilterQuery quoteFilterQuery, SecurityType securityType, Pageable pageable)
    {
        var query = m_Context.Securities.Include(stock => stock.StockExchange)
                             .AsQueryable();

        query = query.Where(security => security.SecurityType == securityType);

        var fromDate = DateOnly.FromDateTime(quoteFilterQuery.Interval switch
                                             {
                                                 QuoteIntervalType.Week        => DateTime.Today.AddDays(-7),
                                                 QuoteIntervalType.Month       => DateTime.Today.AddMonths(-1),
                                                 QuoteIntervalType.ThreeMonths => DateTime.Today.AddMonths(-3),
                                                 QuoteIntervalType.Year        => DateTime.Today.AddYears(-1),
                                                 QuoteIntervalType.Max         => DateTime.MinValue,
                                                 _                             => DateTime.Today
                                             });

        query = query.Include(stock => stock.Quotes.Where(quote => fromDate                               <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                                   DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                            .OrderByDescending(quote => quote.CreatedAt));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            query = query.Where(stock => EF.Functions.ILike(stock.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            query = query.Where(stock => EF.Functions.ILike(stock.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            query = query.Where(stock => stock.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await query.CountAsync();

        var items = await query.Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<Security>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<Security?> FindById(Guid id, QuoteFilterIntervalQuery filter)
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

        return await m_Context.Securities.Include(stock => stock.StockExchange)
                              .Include(stock => stock.Quotes.Where(quote => fromDate                               <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                                            DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                                     .OrderByDescending(quote => quote.CreatedAt))
                              .FirstOrDefaultAsync(stock => stock.Id == id);
    }

    public async Task<Security?> Create(Security security)
    {
        var added = await m_Context.Securities.AddAsync(security);
        await m_Context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<bool> CreateSecurities(List<Security> securities)
    {
        await using var context = await CreateContext;

        await context.Securities.AddRangeAsync(securities);

        return await context.SaveChangesAsync() == securities.Count;
    }
}
