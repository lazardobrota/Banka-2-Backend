using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class StockEntityConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(stock => stock.Id);

        builder.HasAlternateKey(nameof(Stock.Ticker));

        builder.Property(stock => stock.Id)
               .IsRequired();

        builder.Property(stock => stock.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(stock => stock.Ticker)
               .IsRequired()
               .HasMaxLength(8);

        builder.Property(stock => stock.StockExchangeId)
               .IsRequired();

        builder.HasOne(stock => stock.StockExchange)
               .WithMany()
               .HasForeignKey(stock => stock.StockExchangeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(stock => stock.SortedQuotes)
               .WithOne(quote => quote.Stock)
               .HasForeignKey(quote => quote.StockId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
