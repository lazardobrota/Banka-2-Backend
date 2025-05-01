using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Models;
using Bank.ExchangeService.Repositories;
using Bank.Http.Clients.User;

namespace Bank.ExchangeService.Services;

public interface IOrderService
{
    Task<Result<Page<OrderResponse>>> GetAll(OrderFilterQuery orderFilterQuery, Pageable pageable);

    Task<Result<OrderResponse>> GetOne(Guid id);

    Task<Result<OrderResponse>> Create(OrderCreateRequest request);

    Task<Result<OrderResponse>> Update(OrderUpdateRequest request, Guid id);
}

public class OrderService(IOrderRepository orderRepository, ISecurityRepository securityRepository, IUserServiceHttpClient userServiceHttpClient) : IOrderService
{
    private readonly IOrderRepository       m_OrderRepository       = orderRepository;
    private readonly ISecurityRepository    m_SecurityRepository    = securityRepository;
    private readonly IUserServiceHttpClient m_UserServiceHttpClient = userServiceHttpClient;

    public async Task<Result<Page<OrderResponse>>> GetAll(OrderFilterQuery orderFilterQuery, Pageable pageable)
    {
        var orders = await m_OrderRepository.FindAll(orderFilterQuery, pageable);

        var userIds = orders.Items.SelectMany(order => new[] { order.ActuaryId, order.SupervisorId })
                            .Where(userId => userId != null)
                            .Select(userId => userId!.Value)
                            .Distinct()
                            .ToList();

        var userFilter = new UserFilterQuery
                         {
                             Ids = userIds
                         };

        var accountIds = orders.Items.Select(order => order.AccountId)
                               .Select(accountId => accountId)
                               .Distinct()
                               .ToList();

        var accountFilter = new AccountFilterQuery
                            {
                                Ids = accountIds,
                            };

        var userPageTask    = m_UserServiceHttpClient.GetAllUsers(userFilter, Pageable.Create(1,       userIds.Count));
        var accountPageTask = m_UserServiceHttpClient.GetAllAccounts(accountFilter, Pageable.Create(1, accountIds.Count));

        await Task.WhenAll(userPageTask, accountPageTask);

        var userPage    = await userPageTask;
        var accountPage = await accountPageTask;

        var userDictionary    = userPage.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);
        var accountDictionary = accountPage.Items.ToDictionary(accountResponse => accountResponse.Id, accountResponse => accountResponse);

        var result = orders.Items.Select(order => order.ToResponse(userDictionary[order.ActuaryId], order.SupervisorId == null ? null : userDictionary[order.SupervisorId.Value],
                                                                   accountDictionary[order.AccountId]))
                           .ToList();

        return Result.Ok(new Page<OrderResponse>(result, orders.PageNumber, orders.PageSize, orders.TotalElements));
    }

    public async Task<Result<OrderResponse>> GetOne(Guid id)
    {
        var order = await m_OrderRepository.FindById(id);

        if (order is null)
            return Result.NotFound<OrderResponse>();

        var userFilter = new UserFilterQuery
                         {
                             Ids = [order.ActuaryId, order.SupervisorId ?? Guid.Empty]
                         };

        var userPageTask        = m_UserServiceHttpClient.GetAllUsers(userFilter, Pageable.Create(1, 2));
        var accountResponseTask = m_UserServiceHttpClient.GetOneAccount(order.AccountId);

        await Task.WhenAll(userPageTask, accountResponseTask);

        var userPage        = await userPageTask;
        var accountResponse = await accountResponseTask;

        var userDictionary = userPage.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        return Result.Ok(order.ToResponse(userDictionary[order.ActuaryId], order.SupervisorId == null ? null : userDictionary[order.SupervisorId.Value], accountResponse!));
    }

    public async Task<Result<OrderResponse>> Create(OrderCreateRequest request)
    {
        var userFilter = new UserFilterQuery
                         {
                             Ids = [request.ActuaryId, request.SupervisorId]
                         };

        var accountFilter = new AccountFilterQuery
                            {
                                Number = request.AccountNumber.Substring(7, 9)
                            };

        var userPageTask    = m_UserServiceHttpClient.GetAllUsers(userFilter, Pageable.Create(1,       2));
        var accountPageTask = m_UserServiceHttpClient.GetAllAccounts(accountFilter, Pageable.Create(1, 1));

        await Task.WhenAll(userPageTask, accountPageTask);

        var userPage    = await userPageTask;
        var accountPage = await accountPageTask;

        if (request.SupervisorId == Guid.Empty && userPage.PageSize != 1 || request.SupervisorId != Guid.Empty && userPage.PageSize != 2 || accountPage.PageSize != 1)
            return Result.BadRequest<OrderResponse>("Bad request");

        // TODO interval??
        // var security = await m_SecurityRepository.FindById(request.SecurityId, new QuoteFilterIntervalQuery { Interval = QuoteIntervalType.Day  });
        Security security = null!;

        // if (security is null) TODO: Uncomment
        // return Result.BadRequest<OrderResponse>("Could not find Security");

        if (security.SettlementDate != DateOnly.MinValue && security.SettlementDate < DateOnly.FromDateTime(DateTime.Now)) //TODO should decline, check if present
            return Result.BadRequest<OrderResponse>("Security settlement date has passed");

        var userResponses = userPage.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        var order = await m_OrderRepository.Add(request.ToOrder(accountPage.Items[0].Id));

        return Result.Ok(order.ToResponse(userResponses[order.ActuaryId], order.SupervisorId == null ? null : userResponses[order.SupervisorId.Value], accountPage.Items[0]));
    }

    public async Task<Result<OrderResponse>> Update(OrderUpdateRequest request, Guid id)
    {
        var dbOrder = await m_OrderRepository.FindById(id);

        if (dbOrder is null)
            return Result.NotFound<OrderResponse>($"No Order found with Id: {id}");

        var order = await m_OrderRepository.Update(dbOrder.ToOrder(request));

        var userFilter = new UserFilterQuery
                         {
                             Ids = [order.ActuaryId, order.SupervisorId ?? Guid.Empty]
                         };

        var userPageTask        = m_UserServiceHttpClient.GetAllUsers(userFilter, Pageable.Create(1, 2));
        var accountResponseTask = m_UserServiceHttpClient.GetOneAccount(order.AccountId);

        await Task.WhenAll(userPageTask, accountResponseTask);

        var userPage        = await userPageTask;
        var accountResponse = await accountResponseTask;

        var userDictionary = userPage.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        return Result.Ok(order.ToResponse(userDictionary[order.ActuaryId], order.SupervisorId == null ? null : userDictionary[order.SupervisorId.Value], accountResponse!));
    }
}
