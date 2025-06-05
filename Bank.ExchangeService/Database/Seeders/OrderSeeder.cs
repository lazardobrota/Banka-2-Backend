using Bank.Application.Domain;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Database.Seeders;

using OrderModel = Order;

public static partial class Seeder
{
    public static class Order
    {
        public static readonly OrderModel Order01 = new()
                                                    {
                                                        Id            = Guid.Parse("f3ecca8a-f33e-4201-9edd-a323d3e10cf9"),
                                                        ActuaryId     = Guid.Parse("5817c260-e4a9-4dc1-87d9-2fa12af157d9"), //GUID USER
                                                        AccountId     = Guid.Parse("5d5fa996-9533-421c-a319-cd43ff41d86f"), //GUID USER
                                                        SupervisorId  = Guid.Parse("5817c260-e4a9-4dc1-87d9-2fa12af157d9"),
                                                        SecurityId    = Option.AppleCallOption.Id,
                                                        OrderType     = OrderType.Market,
                                                        Quantity      = 0,
                                                        ContractCount = 0,
                                                        LimitPrice    = 0,
                                                        StopPrice     = 0,
                                                        Direction     = Direction.Buy,
                                                        Status        = OrderStatus.Canceled,
                                                        CreatedAt     = DateTime.UtcNow,
                                                        ModifiedAt    = DateTime.UtcNow,
                                                        AllOrNone     = false
                                                    };

        public static readonly OrderModel Order02 = new()
                                                    {
                                                        Id            = Guid.Parse("f1cf9aa0-f48d-447e-98c5-a2bc0c0f79a6"),
                                                        ActuaryId     = Guid.Parse("5817c260-e4a9-4dc1-87d9-2fa12af157d9"), //GUID USER
                                                        AccountId     = Guid.Parse("5d5fa996-9533-421c-a319-cd43ff41d86f"), //GUID USER
                                                        SecurityId    = Option.MicrosoftCallOption.Id,
                                                        OrderType     = OrderType.Market,
                                                        Quantity      = 0,
                                                        ContractCount = 0,
                                                        LimitPrice    = 0,
                                                        StopPrice     = 0,
                                                        Direction     = Direction.Sell,
                                                        Status        = OrderStatus.Active,
                                                        CreatedAt     = DateTime.UtcNow,
                                                        ModifiedAt    = DateTime.UtcNow,
                                                        AllOrNone     = false
                                                    };
    }
}

public static class OrderSeederExtansion
{
    public static async Task SeedOrdersHardcoded(this DatabaseContext context)
    {
        if (context.Orders.Any())
            return;

        await context.Orders.AddRangeAsync(Seeder.Order.Order01, Seeder.Order.Order02);

        await context.SaveChangesAsync();
    }
}
