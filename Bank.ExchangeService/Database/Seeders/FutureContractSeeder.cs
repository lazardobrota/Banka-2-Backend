using Bank.Application.Domain;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Database.Seeders;

using FutureContractModel = FutureContract;

public static partial class Seeder
{
    public static class FutureContract
    {
        public static readonly FutureContractModel CrudeOilFuture = new()
                                                                    {
                                                                        Id              = Guid.Parse("f17ac10b-58cc-4372-a567-0e02b2c3d484"),
                                                                        ContractSize    = 1000,
                                                                        ContractUnit    = ContractUnit.Barrel,
                                                                        SettlementDate  = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                                                                        Name            = "Crude Oil Future March 2024",
                                                                        Ticker          = "CLH24",
                                                                        StockExchangeId = StockExchange.Nasdaq.Id
                                                                    };

        public static readonly FutureContractModel GoldFuture = new()
                                                                {
                                                                    Id              = Guid.Parse("f27ac10b-58cc-4372-a567-0e02b2c3d484"),
                                                                    ContractSize    = 100,
                                                                    ContractUnit    = ContractUnit.Kilogram,
                                                                    SettlementDate  = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)),
                                                                    Name            = "Gold Future June 2024",
                                                                    Ticker          = "GCM24",
                                                                    StockExchangeId = StockExchange.ClearStreet.Id
                                                                };

        public static readonly FutureContractModel CornFuture = new()
                                                                {
                                                                    Id              = Guid.Parse("f37ac10b-58cc-4372-a567-0e02b2c3d484"),
                                                                    ContractSize    = 5000,
                                                                    ContractUnit    = ContractUnit.Kilogram,
                                                                    SettlementDate  = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(4)),
                                                                    Name            = "Corn Future May 2024",
                                                                    Ticker          = "ZCK24",
                                                                    StockExchangeId = StockExchange.BorsaItaliana.Id
                                                                };

        public static readonly FutureContractModel NaturalGasFuture = new()
                                                                      {
                                                                          Id              = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d484"),
                                                                          ContractSize    = 10000,
                                                                          ContractUnit    = ContractUnit.Barrel,
                                                                          SettlementDate  = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)),
                                                                          Name            = "Natural Gas Future February 2024",
                                                                          Ticker          = "NGG24",
                                                                          StockExchangeId = StockExchange.EDGADark.Id
                                                                      };
    }
}

public static class FutureContractSeederExtension
{
    public static async Task SeedFutureContract(this DatabaseContext context)
    {
        if (context.FutureContracts.Any())
            return;

        await context.FutureContracts.AddRangeAsync(Seeder.FutureContract.CornFuture, Seeder.FutureContract.CrudeOilFuture, Seeder.FutureContract.GoldFuture,
                                                    Seeder.FutureContract.NaturalGasFuture);

        await context.SaveChangesAsync();
    }
}
