using Bank.Application.Domain;
using Bank.Application.Queries;
using Bank.Database.Core;
using Bank.ExchangeService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using DatabaseContext = Bank.ExchangeService.Database.DatabaseContext;

namespace Bank.ExchangeService.Repositories;

public interface IOrderRepository
{
    Task<Page<Order>> FindAll(OrderFilterQuery orderFilterQuery, Pageable pageable); // filter

    Task<Order?> FindById(Guid id);

    Task<Order> Add(Order order);

    Task<Order> Update(Order order);

    Task<Order> UpdateStatus(Guid id, OrderStatus status);
}

public class OrderRepository(IAuthorizationService authorizationService, IDatabaseContextFactory<DatabaseContext> contextFactory) : IOrderRepository
{
    private readonly IDatabaseContextFactory<DatabaseContext> m_ContextFactory       = contextFactory;
    private readonly IAuthorizationService                    m_AuthorizationService = authorizationService;

    public async Task<Page<Order>> FindAll(OrderFilterQuery filter, Pageable pageable)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var orderQuery = context.Orders.AsQueryable();

        if (filter.Status != OrderStatus.Invalid)
            orderQuery = orderQuery.Where(order => order.Status == filter.Status);

        var orders = await orderQuery.Skip((pageable.Page - 1) * pageable.Size)
                                     .Take(pageable.Size)
                                     .ToListAsync();

        var totalElements = await orderQuery.CountAsync();

        return new Page<Order>(orders, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Order?> FindById(Guid id)
    {
        await using var context = await m_ContextFactory.CreateContext;

        return await context.Orders.FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<Order> Add(Order order)
    {
        await using var context = await m_ContextFactory.CreateContext;

        var addedOrder = await context.Orders.AddAsync(order);

        await context.SaveChangesAsync();

        return addedOrder.Entity;
    }

    public async Task<Order> Update(Order order)
    {
        await using var context = await m_ContextFactory.CreateContext;

        await context.Orders.Where(dbOrder => dbOrder.Id == order.Id)
                     .ExecuteUpdateAsync(setters => setters.SetProperty(dbOrder => dbOrder.Status, order.Status)
                                                           .SetProperty(dbOrder => dbOrder.ModifiedAt, order.ModifiedAt));

        return order;
    }

    public async Task<Order> UpdateStatus(Guid id, OrderStatus status)
    {
        await using var context = await m_ContextFactory.CreateContext;

        await context.Orders.Where(order => order.Id == id)
                     .ExecuteUpdateAsync(setProperty => setProperty.SetProperty(order => order.Status, status));

        var order = await context.Orders.AsNoTracking()
                                 .FirstOrDefaultAsync(order => order.Id == id);

        return order!;
    }
}
