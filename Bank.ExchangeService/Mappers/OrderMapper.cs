using Bank.Application.Domain;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.ExchangeService.Models;

namespace Bank.ExchangeService.Mappers;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order, UserResponse actuary, UserResponse? supervisor, AccountResponse account)
    {
        return new OrderResponse
               {
                   Id = order.Id,
                   //TODO security no response quite yet, will (not) be
                   Actuary           = actuary,
                   OrderType         = order.OrderType,
                   Quantity          = order.Quantity,
                   ContractCount     = order.ContractCount,
                   PricePerUnit      = order.PricePerUnit,
                   Direction         = order.Direction,
                   Status            = order.Status,
                   Supervisor        = supervisor,
                   Done              = order.Done,
                   RemainingPortions = order.RemainingPortions,
                   AfterHours        = order.AfterHours,
                   CreatedAt         = order.CreatedAt,
                   ModifiedAt        = order.ModifiedAt,
                   Account           = account
               };
    }

    public static Order ToOrder(this OrderCreateRequest createRequest, Guid accountId)
    {
        return new Order
               {
                   Id                = Guid.NewGuid(),
                   ActuaryId         = createRequest.ActuaryId,
                   SupervisorId      = createRequest.SupervisorId == Guid.Empty ? null : createRequest.SupervisorId,
                   OrderType         = createRequest.OrderType,
                   Quantity          = createRequest.Quantity,
                   ContractCount     = createRequest.ContractCount,
                   PricePerUnit      = createRequest.PricePerUnit,
                   Direction         = createRequest.Direction,
                   Status            = OrderStatus.Pending,
                   Done              = false,
                   RemainingPortions = createRequest.Quantity,
                   AfterHours        = createRequest.AfterHours,
                   CreatedAt         = DateTime.UtcNow,
                   ModifiedAt        = DateTime.UtcNow,
                   AccountId         = accountId,
                   SecurityId        = createRequest.SecurityId
               };
    }

    public static Order ToOrder(this Order order, OrderUpdateRequest updateRequest)
    {
        order.Status     = updateRequest.Status;
        order.ModifiedAt = DateTime.UtcNow;
        return order;
    }
}
