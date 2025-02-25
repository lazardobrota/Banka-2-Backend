using Bank.Application.Domain;
using Bank.Application.Utilities;
using Bank.UserService.Models;

namespace Bank.UserService.Database;

public static class DatabaseSeeders
{
    public static async Task SeedUsers(this ApplicationContext context)
    {
        if (context.Users.Any())
            return;

        //TODO: add User seed
        List<User> users =
        [
            new()
            {
                Id                         = Guid.NewGuid(),
                FirstName                  = "Admin",
                LastName                   = "Admin",
                DateOfBirth                = new DateOnly(1990, 1, 1),
                Gender                     = Gender.Male,
                UniqueIdentificationNumber = "0101990710024",
                Email                      = "admin@gmail.com",
                Password                   = "admin",
                Username                   = "Admin123",
                PhoneNumber                = "+38164123456",
                Address                    = "Admin 01",
                Salt                       = Guid.NewGuid(),
                Role                       = Role.Admin,
                Department                 = "Department 01",
                CreatedAt                  = DateTime.UtcNow,
                ModifiedAt                 = DateTime.UtcNow,
                Employed                   = true,
                Activated                  = true
            },
            new()
            {
                Id                         = Guid.NewGuid(),
                FirstName                  = "Client",
                LastName                   = "Client",
                DateOfBirth                = new DateOnly(1997, 11, 11),
                Gender                     = Gender.Male,
                UniqueIdentificationNumber = "1111997730024",
                Email                      = "client@gmail.com",
                Password                   = "client",
                Username                   = "Client",
                PhoneNumber                = "+381640000000",
                Address                    = "Client 01",
                Salt                       = Guid.NewGuid(),
                Role                       = Role.Client,
                CreatedAt                  = DateTime.UtcNow,
                ModifiedAt                 = DateTime.UtcNow,
                Employed                   = true,
                Activated                  = true
            },
            new()
            {
                Id                         = Guid.NewGuid(),
                FirstName                  = "Employee",
                LastName                   = "Employeec",
                DateOfBirth                = new DateOnly(2000, 5, 5),
                Gender                     = Gender.Female,
                UniqueIdentificationNumber = "0505000795024",
                Email                      = "employee@gmail.com",
                Username                   = "employee000",
                Password                   = "12345678",
                PhoneNumber                = "+38164123456",
                Address                    = "Employee 01",
                Salt                       = Guid.NewGuid(),
                Role                       = Role.Employee,
                Department                 = "Department 03",
                CreatedAt                  = DateTime.UtcNow,
                ModifiedAt                 = DateTime.UtcNow,
                Employed                   = true,
                Activated                  = true
            },
        ];

        users = users.Select(user =>
                             {
                                 user.Password = HashingUtilities.HashPassword(user.Password!, user.Salt);
                                 return user;
                             })
                     .ToList();

        await context.Users.AddRangeAsync(users);

        await context.SaveChangesAsync();
    }

    public static async Task SeedAccounts(this ApplicationContext context)
    {
        if (context.Accounts.Any())
            return;

        //TODO: add Account seed
    }
}
