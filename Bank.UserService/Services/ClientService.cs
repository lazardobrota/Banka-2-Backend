using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IClientService
{
    Task<Result<List<ClientResponse>>> FindAll(UserFilterQuery userFilterQuery, Pageable pageable);

    Task<Result<ClientResponse>> GetOne(Guid id);

    Task<Result<ClientResponse>> Create(ClientCreateRequest clientCreateRequest);

    Task<Result<ClientResponse>> Update(ClientUpdateRequest clientUpdateRequest, Guid id);
}

public class ClientService(IUserRepository repository) : IClientService
{
    private readonly IUserRepository m_UserRepository = repository;

    public async Task<Result<List<ClientResponse>>> FindAll(UserFilterQuery userFilterQuery, Pageable pageable)
    {
        var clients = await m_UserRepository.FindAll(userFilterQuery, pageable);

        if (clients == null)
            return Result.NotFound<List<ClientResponse>>();

        var clientResponses = clients.Where(client => client.Role == Role.Client) // ✅ Filtrira samo klijente sa Role.Client
                                     .Select(client => client.ToClient()
                                                             .ToResponse())
                                     .ToList();

        return clientResponses.Count == 0 ? Result.NotFound<List<ClientResponse>>() : Result.Ok(clientResponses);
    }

    public async Task<Result<ClientResponse>> GetOne(Guid id)
    {
        var user = await m_UserRepository.FindById(id);

        if (user is null || user.Role != Role.Client)
            return Result.NotFound<ClientResponse>($"No Client found with Id: {id}");

        return Result.Ok(user.ToClient()
                             .ToResponse());
    }

    public async Task<Result<ClientResponse>> Create(ClientCreateRequest clientCreateRequest)
    {
        var user = await m_UserRepository.Add(clientCreateRequest.ToClient()
                                                                 .ToUser());

        return Result.Ok(user.ToClient()
                             .ToResponse());
    }

    public async Task<Result<ClientResponse>> Update(ClientUpdateRequest clientUpdateRequest, Guid id)
    {
        var oldUser = await m_UserRepository.FindById(id);

        if (oldUser is null || oldUser.Role != Role.Client)
            return Result.NotFound<ClientResponse>($"No Client found with Id: {id}");

        var user = await m_UserRepository.Update(oldUser, clientUpdateRequest.ToClient(oldUser.ToClient())
                                                                             .ToUser());

        return Result.Ok(user.ToClient()
                             .ToResponse());
    }
}
