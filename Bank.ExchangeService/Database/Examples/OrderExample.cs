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
                                                                      StopPrice     = 222.22m,
                                                                      LimitPrice    = 250.75m,
                                                                      Direction     = Direction.Buy,
                                                                      SupervisorId  = Guid.Parse("e1f3de40-719e-4b5f-8e4d-d42f06e4a411"),
                                                                      AccountNumber = "000000005",
                                                                      SecurityId    = Guid.Parse("9000bb03-afac-4ab5-80f3-980a0ed898f2")
                                                                  };

        public static readonly OrderUpdateRequest UpdateRequest = new()
                                                                  {
                                                                      Status = OrderStatus.NeedsApproval
                                                                  };

        public static readonly OrderResponse Response = new()
                                                        {
                                                            Id            = Guid.NewGuid(),
                                                            Actuary       = null!,
                                                            OrderType     = CreateRequest.OrderType,
                                                            Quantity      = CreateRequest.Quantity,
                                                            ContractCount = CreateRequest.ContractCount,
                                                            StopPrice     = CreateRequest.StopPrice,
                                                            LimitPrice    = CreateRequest.LimitPrice,
                                                            Direction     = CreateRequest.Direction,
                                                            Status        = UpdateRequest.Status,
                                                            Supervisor    = null!,
                                                            Account       = null!,
                                                            CreatedAt     = DateTime.UtcNow,
                                                            ModifiedAt    = DateTime.UtcNow,
                                                        };
    }
}
