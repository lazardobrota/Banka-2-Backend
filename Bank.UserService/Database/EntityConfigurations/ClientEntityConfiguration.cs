using Bank.UserService.Models;

namespace Bank.UserService.Database.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(client => client.Id);

        builder.Property(client => client.Id)
               .IsRequired();

        builder.Property(client => client.FirstName)
               .HasMaxLength(32)
               .IsRequired();

        builder.Property(client => client.LastName)
               .HasMaxLength(32)
               .IsRequired();

        builder.Property(client => client.DateOfBirth)
               .IsRequired();

        builder.Property(client => client.Gender)
               .IsRequired();

        builder.Property(client => client.UniqueIdentificationNumber)
               .HasMaxLength(13)
               .IsRequired();

        //Ask should it be unique?
        builder.Property(client => client.Email)
               .HasMaxLength(320)
               .IsRequired();

        builder.Property(client => client.PhoneNumber)
               .HasMaxLength(13)
               .IsRequired();

        builder.Property(client => client.Address)
               .HasMaxLength(64)
               .IsRequired();

        builder.Property(client => client.Password)
               .HasMaxLength(64)
               .IsFixedLength()
               .IsRequired(false);

        builder.Property(client => client.Salt)
               .IsRequired();

        builder.Property(client => client.Role)
               .IsRequired();

        builder.Property(client => client.CreatedAt)
               .IsRequired();

        builder.Property(client => client.ModifiedAt)
               .IsRequired();

        builder.Property(client => client.Activated)
               .IsRequired();
    }
}
