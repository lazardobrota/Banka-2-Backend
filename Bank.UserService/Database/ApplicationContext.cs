using Bank.UserService.Database.EntityConfigurations;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Database;

public class ApplicationContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User>    Users    { init; get; }
    public DbSet<Account> Accounts { init; get; }
    public DbSet<Client>  Clients  { init; get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserEntityConfiguration());
        builder.ApplyConfiguration(new AccountEntityConfiguration());
        builder.ApplyConfiguration(new ClientEntityConfiguration());
    }
}
