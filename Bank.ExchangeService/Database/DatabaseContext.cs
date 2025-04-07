using Bank.ExchangeService.Database.EntityConfigurations;
using Bank.ExchangeService.Model;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Database;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<StockExchange> StockExchanges { init; get; }
    public DbSet<Quote>         Quotes         { init; get; }
    public DbSet<Security>      Securities     { init; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new StockExchangeEntityConfiguration());
        builder.ApplyConfiguration(new QuoteEntityConfiguration());
        builder.ApplyConfiguration(new SecurityEntityConfiguration());
    }
}
