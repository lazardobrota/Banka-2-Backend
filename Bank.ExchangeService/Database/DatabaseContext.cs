using Bank.ExchangeService.Database.EntityConfigurations;
using Bank.ExchangeService.Model;
using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Database;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<StockExchange>  StockExchanges  { init; get; }
    public DbSet<Quote>          Quotes          { init; get; }
    public DbSet<Stock>          Stocks          { init; get; }
    public DbSet<Option>         Options         { init; get; }
    public DbSet<FutureContract> FutureContracts { init; get; }
    public DbSet<ForexPair>      ForexPairs      { init; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new StockExchangeEntityConfiguration());
        builder.ApplyConfiguration(new QuoteEntityConfiguration());
        builder.ApplyConfiguration(new StockEntityConfiguration());
        builder.ApplyConfiguration(new OptionEntityConfiguration());
        builder.ApplyConfiguration(new ForexPairEntityConfiguration());
        builder.ApplyConfiguration(new FutureContractEntityConfiguration());
    }
}
