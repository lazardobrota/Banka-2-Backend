using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class QuoteEntityConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.HasKey(quote => quote.Id);

        builder.Property(quote => quote.Id)
               .IsRequired();

        builder.Property(quote => quote.Price)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(quote => quote.HighPrice)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(quote => quote.LowPrice)
               .IsRequired()
               .HasPrecision(18, 6);

        builder.Property(quote => quote.Volume)
               .IsRequired();

        builder.Property(quote => quote.CreatedAt)
               .IsRequired();

        builder.Property(quote => quote.ModifiedAt)
               .IsRequired();

        builder.Property(quote => quote.SecurityId)
               .IsRequired();

        builder.HasOne(quote => quote.Security)
               .WithMany()
               .HasForeignKey(quote => quote.SecurityId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
