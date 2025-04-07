using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class ForexPairEntityConfiguration : IEntityTypeConfiguration<ForexPair>
{
    public void Configure(EntityTypeBuilder<ForexPair> builder)
    {
        builder.HasKey(forexPair => forexPair.Id);

        builder.Property(forexPair => forexPair.Id)
               .IsRequired();

        builder.Property(forexPair => forexPair.BaseCurrencyId)
               .IsRequired();

        builder.Property(forexPair => forexPair.ContractSize)
               .IsRequired();

        builder.Property(forexPair => forexPair.Name)
               .IsRequired()
               .HasMaxLength(60);

        builder.Property(forexPair => forexPair.Ticker)
               .IsRequired()
               .HasMaxLength(8);

        builder.Property(forexPair => forexPair.StockExchangeId)
               .IsRequired();

        builder.HasOne(forexPair => forexPair.StockExchange)
               .WithMany()
               .HasForeignKey(stock => stock.StockExchangeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(forexPair => forexPair.Liquidity)
               .IsRequired();

        builder.Property(forexPair => forexPair.ExchangeRate)
               .IsRequired();

        builder.Property(forexPair => forexPair.QuoteCurrencyId)
               .IsRequired();

        builder.HasOne(forexPair => forexPair.StockExchange)
               .WithMany()
               .HasForeignKey(forexPair => forexPair.StockExchangeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(forexPair => forexPair.SortedQuotes)
               .WithOne(quote => quote.ForexPair)
               .HasForeignKey(quote => quote.ForexPairId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
