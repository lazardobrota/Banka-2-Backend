using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class QuoteEntityConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.HasKey(history => history.Id);

        builder.Property(history => history.Id)
               .IsRequired();

        builder.Property(history => history.Price)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(history => history.HighPrice)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(history => history.LowPrice)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(history => history.Volume)
               .IsRequired();

        builder.Property(history => history.CreatedAt)
               .IsRequired();

        builder.Property(history => history.ModifiedAt)
               .IsRequired();

        builder.Property(history => history.StockId);
        builder.Property(history => history.ForexPairId);
        builder.Property(history => history.FuturesContractId);
        builder.Property(history => history.OptionId);

        builder.HasOne(history => history.Stock)
               .WithMany()
               .HasForeignKey(history => history.StockId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(history => history.ForexPair)
               .WithMany()
               .HasForeignKey(history => history.ForexPairId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(history => history.FuturesContract)
               .WithMany()
               .HasForeignKey(history => history.FuturesContractId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(history => history.Option)
               .WithMany()
               .HasForeignKey(history => history.OptionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(history => history.StockId);
    }
}
