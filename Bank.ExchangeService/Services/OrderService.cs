using System.Web;

using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.ExchangeService.Configurations;
using Bank.ExchangeService.Mappers;
using Bank.ExchangeService.Repositories;

namespace Bank.ExchangeService.Services;

public interface IOrderService
{
    Task<Result<Page<OrderResponse>>> GetAll(OrderFilterQuery orderFilterQuery, Pageable pageable);

    Task<Result<OrderResponse>> GetOne(Guid id);

    Task<Result<OrderResponse>> Create(OrderCreateRequest request);

    Task<Result<OrderResponse>> Update(OrderUpdateRequest request, Guid id);
}

public class OrderService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory) : IOrderService
{
    private readonly IOrderRepository   m_OrderRepository   = orderRepository;
    private readonly IHttpClientFactory m_HttpClientFactory = httpClientFactory;

    public async Task<Result<Page<OrderResponse>>> GetAll(OrderFilterQuery orderFilterQuery, Pageable pageable)
    {
        var orders = await m_OrderRepository.FindAll(orderFilterQuery, pageable);

        using var httpClient = m_HttpClientFactory.CreateClient(Configuration.HttpClient.Name.UserService);

        var query = orders.Items.SelectMany(order => new[] { order.ActuaryId, order.SupervisorId })
                          .Where(userId => userId != null)
                          .Distinct()
                          .Aggregate(HttpUtility.ParseQueryString(string.Empty), (result, userId) =>
                                                                                 {
                                                                                     result.Add(nameof(UserFilterQuery.Ids), userId.ToString());
                                                                                     return result;
                                                                                 });

        query.Add(nameof(Pageable.Page), "1");
        query.Add(nameof(Pageable.Size), (pageable.Size * 2).ToString());

        var response     = await httpClient.GetAsync($"{Endpoints.User.GetAll}?{query}");
        var responsePage = await response.Content.ReadFromJsonAsync<Page<UserResponse>>();

        var userResponses = responsePage?.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        var result = orders.Items.Select(order => order.ToResponse(userResponses![order.ActuaryId], order.SupervisorId == null ? null : userResponses[order.SupervisorId.Value]))
                           .ToList();

        return Result.Ok(new Page<OrderResponse>(result, orders.PageNumber, orders.PageSize, orders.TotalElements));
    }

    public async Task<Result<OrderResponse>> GetOne(Guid id)
    {
        var order = await m_OrderRepository.FindById(id);

        if (order == null)
            return Result.NotFound<OrderResponse>();

        using var httpClient = m_HttpClientFactory.CreateClient(Configuration.HttpClient.Name.UserService);

        var query = HttpUtility.ParseQueryString(string.Empty);

        query.Add(nameof(UserFilterQuery.Ids), order.ActuaryId.ToString());
        query.Add(nameof(UserFilterQuery.Ids), order.SupervisorId.ToString());
        query.Add(nameof(Pageable.Page),       "1");
        query.Add(nameof(Pageable.Size),       "2");

        var response     = await httpClient.GetAsync($"{Endpoints.User.GetAll}?{query}");
        var responsePage = await response.Content.ReadFromJsonAsync<Page<UserResponse>>();

        var userResponses = responsePage?.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        return Result.Ok(order.ToResponse(userResponses![order.ActuaryId], order.SupervisorId == null ? null : userResponses[order.SupervisorId.Value]));
    }

    public async Task<Result<OrderResponse>> Create(OrderCreateRequest request)
    {
        using var httpClient = m_HttpClientFactory.CreateClient(Configuration.HttpClient.Name.UserService);

        var query = HttpUtility.ParseQueryString(string.Empty);

        query.Add(nameof(UserFilterQuery.Ids), request.ActuaryId.ToString());
        query.Add(nameof(UserFilterQuery.Ids), request.SupervisorId.ToString());
        query.Add(nameof(Pageable.Page),       "1");
        query.Add(nameof(Pageable.Size),       "2");

        var response     = await httpClient.GetAsync($"{Endpoints.User.GetAll}?{query}");
        var responsePage = await response.Content.ReadFromJsonAsync<Page<UserResponse>>();
        // b5d36c22-3b6c-4de0-845b-a1a74e7b9856
        // 3d9bd4c3-a467-4676-ac4b-2b392e7315fa

        if (request.SupervisorId == Guid.Empty && responsePage?.PageSize != 1 || request.SupervisorId != Guid.Empty && responsePage?.PageSize != 2)
            return Result.BadRequest<OrderResponse>("Could not find Supervisor or Actuary");

        var userResponses = responsePage?.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        var order = await m_OrderRepository.Add(request.ToOrder());

        return Result.Ok(order.ToResponse(userResponses![order.ActuaryId], order.SupervisorId == null ? null : userResponses[order.SupervisorId.Value]));
    }

    public async Task<Result<OrderResponse>> Update(OrderUpdateRequest request, Guid id)
    {
        var dbOrder = await m_OrderRepository.FindById(id);

        if (dbOrder is null)
            return Result.NotFound<OrderResponse>($"No Order found with Id: {id}");

        var order = await m_OrderRepository.Update(dbOrder.ToOrder(request));

        using var httpClient = m_HttpClientFactory.CreateClient(Configuration.HttpClient.Name.UserService);

        var query = HttpUtility.ParseQueryString(string.Empty);

        query.Add(nameof(UserFilterQuery.Ids), order.ActuaryId.ToString());
        query.Add(nameof(UserFilterQuery.Ids), order.SupervisorId.ToString());
        query.Add(nameof(Pageable.Page),       "1");
        query.Add(nameof(Pageable.Size),       "2");

        var response     = await httpClient.GetAsync($"{Endpoints.User.GetAll}?{query}");
        var responsePage = await response.Content.ReadFromJsonAsync<Page<UserResponse>>();

        var userResponses = responsePage?.Items.ToDictionary(userResponse => userResponse.Id, userResponse => userResponse);

        return Result.Ok(order.ToResponse(userResponses![order.ActuaryId], order.SupervisorId == null ? null : userResponses[order.SupervisorId.Value]));
    }
}
