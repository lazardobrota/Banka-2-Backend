using Microsoft.EntityFrameworkCore;

namespace Bank.ExchangeService.Database;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder) { }
}
