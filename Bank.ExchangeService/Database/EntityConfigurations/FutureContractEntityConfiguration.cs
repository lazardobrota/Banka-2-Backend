using Bank.ExchangeService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.ExchangeService.Database.EntityConfigurations;

public class FutureContractEntityConfiguration : IEntityTypeConfiguration<FutureContract>
{
    public void Configure(EntityTypeBuilder<FutureContract> builder)
    {
        builder.HasKey(futureContract => futureContract.Id);

        builder.Property(futureContract => futureContract.Id)
               .IsRequired();

        builder.Property(futureContract => futureContract.ContractUnit)
               .IsRequired();

        builder.Property(futureContract => futureContract.SettlementDate)
               .IsRequired();

        builder.Property(futureContract => futureContract.Name)
               .IsRequired()
               .HasMaxLength(60);

        builder.Property(futureContract => futureContract.Ticker)
               .IsRequired()
               .HasMaxLength(8);

        builder.Property(futureContract => futureContract.StockExchangeId)
               .IsRequired();

        builder.HasOne(futureContract => futureContract.StockExchange)
               .WithMany()
               .HasForeignKey(futureContract => futureContract.StockExchangeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(futureContract => futureContract.SortedQuotes)
               .WithOne(quote => quote.FuturesContract)
               .HasForeignKey(quote => quote.FuturesContractId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
