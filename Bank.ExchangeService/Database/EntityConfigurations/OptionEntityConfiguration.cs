using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class OptionEntityConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(option => option.Id);

        builder.Property(option => option.Id)
               .IsRequired();

        builder.Property(option => option.OptionType)
               .IsRequired();

        builder.Property(option => option.StrikePrice)
               .IsRequired();

        builder.Property(option => option.ImpliedVolatility)
               .IsRequired();

        builder.Property(option => option.OpenInterest)
               .IsRequired();

        builder.Property(option => option.SettlementDate)
               .IsRequired();

        builder.Property(option => option.Name)
               .IsRequired()
               .HasMaxLength(64);

        builder.Property(option => option.Ticker)
               .IsRequired()
               .HasMaxLength(19);

        builder.Property(option => option.StockExchangeId)
               .IsRequired();

        builder.HasOne(option => option.StockExchange)
               .WithMany()
               .HasForeignKey(option => option.StockExchangeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(option => option.SortedQuotes)
               .WithOne(quote => quote.Option)
               .HasForeignKey(quote => quote.OptionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
