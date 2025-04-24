using Bank.Database.Core;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

using DatabaseContext = Bank.ExchangeService.Database.DatabaseContext;

namespace Bank.ExchangeService.Repositories;

public interface IQuoteRepository
{
    Task<bool> CreateQuotes(List<Quote> quotes);
}

public class QuoteRepository(IDatabaseContextFactory<DatabaseContext> contextFactory) : IQuoteRepository
{
    private readonly IDatabaseContextFactory<DatabaseContext> m_ContextFactory = contextFactory;
    
    public async Task<bool> CreateQuotes(List<Quote> quotes)
    {
        await using var context = await m_ContextFactory.CreateContext;

        await context.Quotes.AddRangeAsync(quotes);

        return await context.SaveChangesAsync() == quotes.Count;
    }
}
