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
                   LimitPrice        = order.LimitPrice,
                   StopPrice         = order.StopPrice,
                   Direction         = order.Direction,
                   Status            = order.Status,
                   Supervisor        = supervisor,
                   CreatedAt         = order.CreatedAt,
                   ModifiedAt        = order.ModifiedAt,
                   Account           = account
               };
    }

    public static RedisOrder ToRedis(this Order order)
    {
        return new RedisOrder
               {
                   Id                = order.Id,
                   Ticker            = order.Security!.Ticker, //TODO: make sure its not null
                   OrderType         = order.OrderType,
                   RemainingPortions = order.Quantity,
                   LimitPrice        = order.LimitPrice,
                   StopPrice         = order.StopPrice,
                   Direction         = order.Direction,
                   AccountId         = order.AccountId
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
                   LimitPrice        = createRequest.LimitPrice,
                   StopPrice         = createRequest.StopPrice,
                   Direction         = createRequest.Direction,
                   Status            = OrderStatus.Active,
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
