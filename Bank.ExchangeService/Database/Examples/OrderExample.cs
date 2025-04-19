using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.ExchangeService.Database.Examples;

public static partial class Example
{
    public static class Order
    {
        public static readonly OrderCreateRequest CreateRequest = new()
                                                                  {
                                                                      ActuaryId     = Guid.Parse("a8be210e-84f9-472e-9f0a-f2f334dcb20a"),
                                                                      OrderType     = OrderType.Market,
                                                                      Quantity      = 100,
                                                                      ContractCount = 10,
                                                                      PricePerUnit  = 250.75m,
                                                                      Direction     = Direction.Buy,
                                                                      SupervisorId  = Guid.Parse("e1f3de40-719e-4b5f-8e4d-d42f06e4a411"),
                                                                      AfterHours    = false,
                                                                      AccountNumber = "1234567890",
                                                                      SecurityId    = Guid.Parse("b2c3d4e5-f6a7-8b9c-a0d1-e2f3g4h5i6j7")
                                                                  };

        public static readonly OrderUpdateRequest UpdateRequest = new()
                                                                  {
                                                                      Status = OrderStatus.Approved
                                                                  };

        public static readonly OrderResponse Response = new()
                                                        {
                                                            Id            = Guid.NewGuid(),
                                                            Actuary       = null!,
                                                            OrderType     = CreateRequest.OrderType,
                                                            Quantity      = CreateRequest.Quantity,
                                                            ContractCount = CreateRequest.ContractCount,
                                                            PricePerUnit  = CreateRequest.PricePerUnit,
                                                            Direction     = CreateRequest.Direction,
                                                            Status        = UpdateRequest.Status,
                                                            Supervisor    = null!,
                                                            Done          = true,
                                                            Account       = null!,
                                                            AfterHours    = CreateRequest.AfterHours,
                                                            CreatedAt     = DateTime.UtcNow,
                                                            ModifiedAt    = DateTime.UtcNow,
                                                            RemainingPortions = 0
                                                        };
    }
}
