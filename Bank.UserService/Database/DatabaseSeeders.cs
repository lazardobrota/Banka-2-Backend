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

        var now   = DateTime.UtcNow;
        var users = new List<User>();

        // Admin 
        User admin = new()
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
                     };

        users.Add(admin);

        var employee1 = new User
                        {
                            Id                         = Guid.NewGuid(),
                            FirstName                  = "John",
                            LastName                   = "Smith",
                            DateOfBirth                = new DateOnly(1990, 7, 20),
                            Gender                     = Gender.Male,
                            UniqueIdentificationNumber = "2007990500012",
                            Email                      = "employee1@gmail.com",
                            Username                   = "employee1",
                            PhoneNumber                = "+38162345678",
                            Address                    = "Oak Avenue 45",
                            Password                   = "employee1",
                            Salt                       = Guid.NewGuid(),
                            Role                       = Role.Employee,
                            Department                 = "Customer Service",
                            CreatedAt                  = now,
                            ModifiedAt                 = now,
                            Employed                   = true,
                            Activated                  = true
                        };

        users.Add(employee1);

        var employee2 = new User
                        {
                            Id                         = Guid.Parse("f63d4dae-b9d7-4d5a-9d5a-6b831c7e8b9a"),
                            FirstName                  = "Maria",
                            LastName                   = "Jones",
                            DateOfBirth                = new DateOnly(1988, 3, 12),
                            Gender                     = Gender.Female,
                            UniqueIdentificationNumber = "1203988715015",
                            Email                      = "employee2@bankapp.com",
                            Username                   = "maria.jones",
                            PhoneNumber                = "+38163456789",
                            Address                    = "Pine Street 78",
                            Password                   = "employee2",
                            Salt                       = Guid.NewGuid(),
                            Role                       = Role.Employee,
                            Department                 = "Loans",
                            CreatedAt                  = now,
                            ModifiedAt                 = now,
                            Employed                   = true,
                            Activated                  = true,
                            Accounts                   = []
                        };

        users.Add(employee2);

        var employee3 = new User
                        {
                            Id                         = Guid.Parse("9e2b3a6c-7d8e-4f1a-b2c3-d4e5f6a7b8c9"),
                            FirstName                  = "Stefan",
                            LastName                   = "Nikolic",
                            DateOfBirth                = new DateOnly(1992, 6, 5),
                            Gender                     = Gender.Male,
                            UniqueIdentificationNumber = "0506992170005",
                            Email                      = "employee3@bankapp.com",
                            Username                   = "employee3",
                            PhoneNumber                = "+38164987654",
                            Address                    = "Knez Mihailova 22",
                            Password                   = "employee3",
                            Salt                       = Guid.NewGuid(),
                            Role                       = Role.Employee,
                            Department                 = "IT",
                            CreatedAt                  = now,
                            ModifiedAt                 = now,
                            Employed                   = true,
                            Activated                  = true,
                            Accounts                   = []
                        };

        users.Add(employee3);

        var employee4 = new User
                        {
                            Id                         = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                            FirstName                  = "Ana",
                            LastName                   = "Marković",
                            DateOfBirth                = new DateOnly(1991, 8, 15),
                            Gender                     = Gender.Female,
                            UniqueIdentificationNumber = "1508991785013",
                            Email                      = "employee4@gmail.com",
                            Username                   = "ana.markovic",
                            PhoneNumber                = "+38165234567",
                            Address                    = "Bulevar Kralja Aleksandra 125",
                            Password                   = "employee4",
                            Salt                       = Guid.NewGuid(),
                            Role                       = Role.Employee,
                            Department                 = "Human Resources",
                            CreatedAt                  = now,
                            ModifiedAt                 = now,
                            Employed                   = true,
                            Activated                  = true,
                            Accounts                   = []
                        };

        users.Add(employee4);

        var client1 = new User
                      {
                          Id                         = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"),
                          FirstName                  = "Peter",
                          LastName                   = "Parker",
                          DateOfBirth                = new DateOnly(1992, 8, 10),
                          Gender                     = Gender.Male,
                          UniqueIdentificationNumber = "1008992450037",
                          Email                      = "client1@gmail.com",
                          Username                   = "peter.parker",
                          PhoneNumber                = "+38166567890",
                          Address                    = "Queens Boulevard 20",
                          Password                   = "client1",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client1);

        var client2 = new User
                      {
                          Id                         = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d0e-1f2a3b4c5d6e"),
                          FirstName                  = "Mary",
                          LastName                   = "Watson",
                          DateOfBirth                = new DateOnly(1993, 4, 15),
                          Gender                     = Gender.Female,
                          UniqueIdentificationNumber = "1504993725015",
                          Email                      = "client2@gmail.com",
                          Username                   = "mary.watson",
                          PhoneNumber                = "+38167678901",
                          Address                    = "Manhattan Avenue 30",
                          Password                   = "client2",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client2);

        var client3 = new User
                      {
                          Id                         = Guid.Parse("c3d4e5f6-a7b8-4c5d-9e0f-1a2b3c4d5e6f"),
                          FirstName                  = "Marko",
                          LastName                   = "Jovanović",
                          DateOfBirth                = new DateOnly(1989, 9, 25),
                          Gender                     = Gender.Male,
                          UniqueIdentificationNumber = "2509989300007",
                          Email                      = "client3@gmail.com",
                          Username                   = "marko.jovanovic",
                          PhoneNumber                = "+38168789012",
                          Address                    = "Cara Dušana 55",
                          Password                   = "client3",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client3);

        var client4 = new User
                      {
                          Id                         = Guid.Parse("d4e5f6a7-b8c9-4d5e-9f0a-1b2c3d4e5f6a"),
                          FirstName                  = "Jelena",
                          LastName                   = "Petrović",
                          DateOfBirth                = new DateOnly(1990, 11, 12),
                          Gender                     = Gender.Female,
                          UniqueIdentificationNumber = "1211990785019",
                          Email                      = "client4@gmail.com",
                          Username                   = "jelena.petrovic",
                          PhoneNumber                = "+38169890123",
                          Address                    = "Bulevar Oslobođenja 71",
                          Password                   = "client4",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client4);

        var client5 = new User
                      {
                          Id                         = Guid.Parse("e5f6a7b8-c9d0-4e5f-9a0b-1c2d3e4f5a6b"),
                          FirstName                  = "Milan",
                          LastName                   = "Đorđević",
                          DateOfBirth                = new DateOnly(1985, 7, 7),
                          Gender                     = Gender.Male,
                          UniqueIdentificationNumber = "0707985120008",
                          Email                      = "client5@gmail.com",
                          Username                   = "milan.djordjevic",
                          PhoneNumber                = "+38161901234",
                          Address                    = "Vojvode Stepe 122",
                          Password                   = "client5",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client5);

        var client6 = new User
                      {
                          Id                         = Guid.Parse("f6a7b8c9-d0e1-4f6a-9b0c-1d2e3f4a5b6c"),
                          FirstName                  = "Nikola",
                          LastName                   = "Stojanović",
                          DateOfBirth                = new DateOnly(1994, 3, 18),
                          Gender                     = Gender.Male,
                          UniqueIdentificationNumber = "1803994430003",
                          Email                      = "client6@gmail.com",
                          Username                   = "nikola.stojanovic",
                          PhoneNumber                = "+38162012345",
                          Address                    = "Nemanjina 18",
                          Password                   = "client6",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client6);

        var client7 = new User
                      {
                          Id                         = Guid.Parse("a7b8c9d0-e1f2-4a7b-9c0d-1e2f3a4b5c6d"),
                          FirstName                  = "Jovana",
                          LastName                   = "Ilić",
                          DateOfBirth                = new DateOnly(1995, 6, 22),
                          Gender                     = Gender.Female,
                          UniqueIdentificationNumber = "2206995705014",
                          Email                      = "client7c@gmail.com",
                          Username                   = "jovana.ilic",
                          PhoneNumber                = "+38163123456",
                          Address                    = "Takovska 7",
                          Password                   = "client7",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client7);

        var client8 = new User
                      {
                          Id                         = Guid.Parse("b8c9d0e1-f2a3-4b8c-9d0e-1f2a3b4c5d6e"),
                          FirstName                  = "Aleksandar",
                          LastName                   = "Todorović",
                          DateOfBirth                = new DateOnly(1987, 9, 3),
                          Gender                     = Gender.Male,
                          UniqueIdentificationNumber = "0309987350002",
                          Email                      = "client8@gmail.com",
                          Username                   = "aleksandar.todorovic",
                          PhoneNumber                = "+38164234567",
                          Address                    = "Kralja Milana 31",
                          Password                   = "client8",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client8);

        var client9 = new User
                      {
                          Id                         = Guid.Parse("c9d0e1f2-a3b4-4c9d-0e1f-2a3b4c5d6e7f"),
                          FirstName                  = "Milica",
                          LastName                   = "Pavlović",
                          DateOfBirth                = new DateOnly(1996, 2, 28),
                          Gender                     = Gender.Female,
                          UniqueIdentificationNumber = "2802996735018",
                          Email                      = "client9@email.com",
                          Username                   = "milica.pavlovic",
                          PhoneNumber                = "+38165345678",
                          Address                    = "Makedonska 15",
                          Password                   = "client9",
                          Salt                       = Guid.NewGuid(),
                          Role                       = Role.Client,
                          Department                 = null,
                          CreatedAt                  = now,
                          ModifiedAt                 = now,
                          Employed                   = null,
                          Activated                  = true
                      };

        users.Add(client9);

        var client10 = new User
                       {
                           Id                         = Guid.Parse("d0e1f2a3-b4c5-4d0e-1f2a-3b4c5d6e7f8a"),
                           FirstName                  = "Dragan",
                           LastName                   = "Simić",
                           DateOfBirth                = new DateOnly(1983, 12, 5),
                           Gender                     = Gender.Male,
                           UniqueIdentificationNumber = "0512983290007",
                           Email                      = "client10@gmail.com",
                           Username                   = "dragan.simic",
                           PhoneNumber                = "+38166456789",
                           Address                    = "Terazije 23",
                           Password                   = "client10",
                           Salt                       = Guid.NewGuid(),
                           Role                       = Role.Client,
                           Department                 = null,
                           CreatedAt                  = now,
                           ModifiedAt                 = now,
                           Employed                   = null,
                           Activated                  = true
                       };

        users.Add(client10);

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

        var list = new List<Account>
                   {
                       new()
                       {
                           Id            = Guid.Parse("1234abcd-5678-4a5b-9c0d-ef0123456789"),
                           AccountNumber = "1234-5678-9012",
                           UserId        = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d"),
                           User          = null
                       },

                       new()
                       {
                           Id            = Guid.Parse("abcd1234-5678-4a5b-9c0d-ef9876543210"),
                           AccountNumber = "2345-6789-0123",
                           UserId        = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d0e-1f2a3b4c5d6e"),
                           User          = null
                       },
                       new()
                       {
                           Id            = Guid.Parse("efab1234-5678-4c5d-9e0f-12345abcdef0"),
                           AccountNumber = "3456-7890-1234",
                           UserId        = Guid.Parse("b2c3d4e5-f6a7-4b5c-9d0e-1f2a3b4c5d6e"),
                           User          = null
                       },

                       new()
                       {
                           Id            = Guid.Parse("1a2b3c4d-5e6f-4a5b-8c9d-1e2f3a4b5c6d"),
                           AccountNumber = "4567-8901-2345",
                           UserId        = Guid.Parse("c3d4e5f6-a7b8-4c5d-9e0f-1a2b3c4d5e6f"),
                           User          = null
                       },

                       new()
                       {
                           Id            = Guid.Parse("2b3c4d5e-6f7a-4b5c-9d0e-1f2a3b4c5d6e"),
                           AccountNumber = "5678-9012-3456",
                           UserId        = Guid.Parse("d4e5f6a7-b8c9-4d5e-9f0a-1b2c3d4e5f6a"),
                           User          = null
                       }
                   };

        await context.Accounts.AddRangeAsync(list);

        await context.SaveChangesAsync();
    }

    private static readonly List<Currency> s_Currencies =
    [
        new()
        {
            Id          = Guid.Parse("b4354f8d-5e1c-48cb-9923-b7139e599558"),
            Name        = "Euro",
            Code        = "EUR",
            Symbol      = "€",
            Countries   = [],
            Description = "The official currency of the Eurozone.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("84ec8c8b-b62c-46c7-b2ab-0f4a4d5930ad"),
            Name        = "Swiss Franc",
            Code        = "CHF",
            Symbol      = "CHF",
            Countries   = [],
            Description = "The official currency of Switzerland.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("7f3d5f0e-4cd6-40a3-bb5a-d8e028d7e77e"),
            Name        = "US Dollar",
            Code        = "USD",
            Symbol      = "$",
            Countries   = [],
            Description = "The official currency of the United States of America.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("0f173c9d-e212-4f8f-b6f5-0e299dbe53ad"),
            Name        = "British Pound",
            Code        = "GBP",
            Symbol      = "£",
            Countries   = [],
            Description = "The official currency of the United Kingdom.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("bcd35b3c-b6fd-45a3-94a0-7a5bdbf6169e"),
            Name        = "Japanese Yen",
            Code        = "JPY",
            Symbol      = "¥",
            Countries   = [],
            Description = "The official currency of Japan.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("ad8797e7-d028-44db-b585-b07b1b7c21c2"),
            Name        = "Canadian Dollar",
            Code        = "CAD",
            Symbol      = "$",
            Countries   = [],
            Description = "The official currency of Canada.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        },
        new()
        {
            Id          = Guid.Parse("7cfcf410-e90b-4a9c-80b0-13215b69c11d"),
            Name        = "Australian Dollar",
            Code        = "AUD",
            Symbol      = "$",
            Countries   = [],
            Description = "The official currency of Australia.",
            Status      = true,
            CreatedAt   = DateTime.UtcNow,
            ModifiedAt  = DateTime.UtcNow
        }
    ];

    public static async Task SeedCurrency(this ApplicationContext context)
    {
        if (context.Currencies.Any())
            return;

        await context.Currencies.AddRangeAsync(s_Currencies);
        await context.SaveChangesAsync();
    }

    private static readonly List<Country> s_Countries =
    [
        new()
        {
            Id         = Guid.Parse("d34c35b7-f438-4b5e-8e26-f3d9122bda7f"),
            Name       = "Germany",
            CurrencyId = s_Currencies[0].Id,
            CreatedAt  = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new()
        {
            Id         = Guid.Parse("d2544b5c-79f4-47a3-8ac7-4d91e6d56fd4"),
            Name       = "Switzerland",
            CurrencyId = s_Currencies[1].Id,
            CreatedAt  = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new()
        {
            Id         = Guid.Parse("f8c9d2b5-cf74-4c2f-8c2e-b0427107a510"),
            Name       = "United States",
            CurrencyId = s_Currencies[2].Id,
            CreatedAt  = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new()
        {
            Id         = Guid.Parse("db5393b1-4f2e-48d0-92bb-df397f08714d"),
            Name       = "United Kingdom",
            CurrencyId = s_Currencies[3].Id,
            CreatedAt  = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new()
        {
            Id         = Guid.Parse("779ff092-3c78-467a-bd5e-39df46a2da3e"),
            Name       = "Japan",
            CurrencyId = s_Currencies[4].Id,
            CreatedAt  = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        }
    ];

    public static async Task SeedCountry(this ApplicationContext context)
    {
        if (context.Countries.Any())
            return;

        await context.Countries.AddRangeAsync(s_Countries);
        await context.SaveChangesAsync();
    }
}
