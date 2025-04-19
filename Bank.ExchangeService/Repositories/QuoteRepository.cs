using Bank.ExchangeService.Database;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Repositories;

public interface IQuoteRepository
{
    Task<bool> CreateQuotes(List<Quote> quotes);
}

public class QuoteRepository(DatabaseContext context, IDbContextFactory<DatabaseContext> contextFactory) : IQuoteRepository
{
    private readonly DatabaseContext                    m_Context        = context;
    private readonly IDbContextFactory<DatabaseContext> m_ContextFactory = contextFactory;

    private Task<DatabaseContext> CreateContext => m_ContextFactory.CreateDbContextAsync();

    public async Task<bool> CreateQuotes(List<Quote> quotes)
    {
        await using var context = await CreateContext;

        await context.Quotes.AddRangeAsync(quotes);

        return await context.SaveChangesAsync() == quotes.Count;
    }
}
