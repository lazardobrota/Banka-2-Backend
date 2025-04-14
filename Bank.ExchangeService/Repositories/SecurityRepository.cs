using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface ISecurityRepository
{
    Task<List<Security>> FindAll(SecurityType securityType);

    Task<Page<Security>> FindAll(QuoteFilterQuery quoteFilterQuery, SecurityType securityType, Pageable pageable, bool inPast = true);

    Task<Security?> FindById(Guid id, QuoteFilterIntervalQuery filter, bool inPast = true);

    Task<Security?> FindByIdDaily(Guid id, QuoteFilterIntervalQuery filter);

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

    public async Task<Page<Security>> FindAll(QuoteFilterQuery quoteFilterQuery, SecurityType securityType, Pageable pageable, bool inPast)
    {
        var query = m_Context.Securities.Include(stock => stock.StockExchange)
                             .AsQueryable();

        query = query.Where(security => security.SecurityType == securityType && security.Quotes.Any());

        var fromDateQuote = CalculateDateInterval(quoteFilterQuery.Interval);

        query = query.Include(stock => stock.Quotes.Where(quote => fromDateQuote                          <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                                   DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                            .OrderByDescending(quote => quote.CreatedAt));

        var date     = CalculateDateInterval(quoteFilterQuery.Interval, inPast);
        var fromDate = inPast ? date : DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var toDate   = inPast ? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) : date;

        if (!string.IsNullOrEmpty(quoteFilterQuery.Name))
            query = query.Where(stock => EF.Functions.ILike(stock.Name, $"%{quoteFilterQuery.Name}%"));

        if (!string.IsNullOrEmpty(quoteFilterQuery.Ticker))
            query = query.Where(stock => EF.Functions.ILike(stock.Ticker, $"%{quoteFilterQuery.Ticker}%"));

        if (quoteFilterQuery.StockExchangeId != Guid.Empty)
            query = query.Where(stock => stock.StockExchangeId == quoteFilterQuery.StockExchangeId);

        var total = await query.CountAsync();

        var items = await query.Where(security => security.SettlementDate == DateOnly.MinValue || fromDate <= security.SettlementDate && security.SettlementDate <= toDate)
                               .AsNoTracking()
                               .OrderByDescending(security => security.Quotes.First()
                                                                      .CreatedAt)
                               .Skip((pageable.Page - 1) * pageable.Size)
                               .Take(pageable.Size)
                               .ToListAsync();

        return new Page<Security>(items, pageable.Page, pageable.Size, total);
    }

    public async Task<Security?> FindById(Guid id, QuoteFilterIntervalQuery filter, bool inPast)
    {
        var date = DateOnly.FromDateTime(filter.Interval switch
                                         {
                                             QuoteIntervalType.Week        => DateTime.Today.AddDays(-7),
                                             QuoteIntervalType.Month       => DateTime.Today.AddMonths(-1),
                                             QuoteIntervalType.ThreeMonths => DateTime.Today.AddMonths(-3),
                                             QuoteIntervalType.Year        => DateTime.Today.AddYears(-1),
                                             QuoteIntervalType.Max         => inPast ? DateTime.MinValue : DateTime.MaxValue,
                                             _                             => inPast ? DateTime.Today : DateTime.Today.AddDays(1)
                                         });

        var fromDate = inPast ? date : DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var toDate   = inPast ? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) : date;

        var query = m_Context.Securities.Include(stock => stock.StockExchange)
                             .AsNoTracking()
                             .Where(stock => stock.Id == id)
                             .AsQueryable();

        if (inPast)
            query = query.Include(security => security.Quotes.Where(quote => fromDate <= DateOnly.FromDateTime(quote.CreatedAt) && DateOnly.FromDateTime(quote.CreatedAt) <= toDate)
                                                      .OrderByDescending(quote => quote.CreatedAt));

        return await query.FirstOrDefaultAsync(stock => stock.Id == id);
    }

    public async Task<Security?> FindByIdDaily(Guid id, QuoteFilterIntervalQuery filter)
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

        var security = await m_Context.Securities.Include(security => security.StockExchange)
                                      .FirstOrDefaultAsync(security => security.Id == id);

        if (security is null)
            return null;

        var quotes = await m_Context.Quotes.AsNoTracking()
                                    .Where(quote => quote.SecurityId == id)
                                    .Where(quote => fromDate                               <= DateOnly.FromDateTime(quote.CreatedAt) &&
                                                    DateOnly.FromDateTime(quote.CreatedAt) <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                                    .GroupBy(quote => quote.CreatedAt.Date)
                                    .Select(group => new DailyQuote
                                                     {
                                                         HighPrice = group.Max(quote => quote.HighPrice),
                                                         LowPrice  = group.Min(quote => quote.LowPrice),
                                                         ClosePrice = group.OrderBy(quote => quote.CreatedAt)
                                                                           .Last()
                                                                           .ClosePrice,
                                                         OpeningPrice = group.OrderBy(quote => quote.CreatedAt)
                                                                             .First()
                                                                             .OpeningPrice,
                                                         Volume = group.Sum(quote => quote.Volume),
                                                         Date   = group.Key.Date
                                                     })
                                    .OrderByDescending(candle => candle.Date)
                                    .ToListAsync();

        security.DailyQuotes = quotes;
        return security;
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

    private DateOnly CalculateDateInterval(QuoteIntervalType intervalType, bool inPast = true)
    {
        var inPastValue = inPast ? -1 : 1;

        return DateOnly.FromDateTime(intervalType switch
                                     {
                                         QuoteIntervalType.Day         => inPast ? DateTime.Today : DateTime.Today.AddDays(1),
                                         QuoteIntervalType.Week        => DateTime.Today.AddDays(7   * inPastValue),
                                         QuoteIntervalType.Month       => DateTime.Today.AddMonths(1 * inPastValue),
                                         QuoteIntervalType.ThreeMonths => DateTime.Today.AddMonths(3 * inPastValue),
                                         QuoteIntervalType.Year        => DateTime.Today.AddYears(1  * inPastValue),
                                         QuoteIntervalType.Max         => inPast ? DateTime.MinValue : DateTime.MaxValue,
                                         _                             => DateTime.Today.AddDays(7 * inPastValue)
                                     });
    }
}
