using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.UserService.Database.EntityConfigurations;

public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(account => account.Id);

        builder.Property(account => account.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(account => account.AccountNumber)
               .IsRequired()
               .HasMaxLength(16);

        builder.Property(account => account.UserId)
               .IsRequired();

        builder.HasOne(account => account.User)
               .WithMany(user => user.Accounts)
               .HasForeignKey(account => account.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
