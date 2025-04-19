using Bank.Application.Requests;
using Bank.Application.Responses;

namespace Bank.OpenApi.Examples;

public static partial class Example
{
    public static class Order
    {
        public static readonly OrderCreateRequest DefaultCreateRequest = new()
                                                                         {
                                                                             ActuaryId     = Constant.Id,
                                                                             OrderType     = Constant.OrderType,
                                                                             Quantity      = Constant.Quantity,
                                                                             ContractCount = Constant.ContractCount,
                                                                             PricePerUnit  = Constant.PricePerUnit,
                                                                             Direction     = Constant.Direction,
                                                                             SupervisorId  = Constant.Id,
                                                                             AfterHours    = Constant.Boolean,
                                                                             AccountNumber = Constant.AccountNumber,
                                                                             SecurityId    = Constant.Id
                                                                         };

        public static readonly OrderUpdateRequest DefaultUpdateRequest = new()
                                                                         {
                                                                             Status = Constant.OrderStatus
                                                                         };

        public static readonly OrderResponse DefaultResponse = new()
                                                               {
                                                                   Id                = Constant.Id,
                                                                   Actuary           = User.DefaultResponse,
                                                                   OrderType         = Constant.OrderType,
                                                                   Quantity          = Constant.Quantity,
                                                                   ContractCount     = Constant.ContractCount,
                                                                   PricePerUnit      = Constant.PricePerUnit,
                                                                   Direction         = Constant.Direction,
                                                                   Supervisor        = User.DefaultResponse,
                                                                   RemainingPortions = Constant.RemainingPortions,
                                                                   Done              = Constant.Boolean,
                                                                   AfterHours        = Constant.Boolean,
                                                                   Status            = Constant.OrderStatus,
                                                                   CreatedAt         = DateTime.Now,
                                                                   ModifiedAt        = DateTime.Now,
                                                                   Account           = Account.DefaultResponse,
                                                               };
    }
}
