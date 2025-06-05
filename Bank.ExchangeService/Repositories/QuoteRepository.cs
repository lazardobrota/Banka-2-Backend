using Bank.Database.Core;
using Bank.ExchangeService.Models;

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

// public static partial class RepositoryExtensions
// {
//     public static IIncludableQueryable<Quote, object?> IncludeAll(this DbSet<Quote> set)
//     {
//         return set.Include(quote => quote.Security)
//                   .ThenIncludeAll(quote => quote.Security, nameof(Security.Quotes));
//     }
//
//     public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, Quote?> value,
//                                                                                  Expression<Func<TEntity, Quote?>> navigationExpression, params string[] excludeProperties)
//     where TEntity : class
//     {
//         IIncludableQueryable<TEntity, object?> query = value;
//     
//         if (!excludeProperties.Contains(nameof(Quote.Security)))
//             query = query.Include(navigationExpression)
//                          .ThenInclude(quote =>    quote!.Security);
//     
//         return query;
//     }
//
//     public static IIncludableQueryable<TEntity, object?> ThenIncludeAll<TEntity>(this IIncludableQueryable<TEntity, List<Quote>>value,
//                                                                                  Expression<Func<TEntity, List<Quote>>> navigationExpression, params string[] excludeProperties)
//     where TEntity : class
//     {
//         IIncludableQueryable<TEntity, object?> query = value;
//
//         if (!excludeProperties.Contains(nameof(Quote.Security)))
//             query = query.Include(navigationExpression)
//                          .ThenInclude(quote =>    quote.Security);
//
//         return query;
//     }
// }