using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;

using DefaultNamespace;

namespace Bank.UserService.Services;

public interface IClientService
{
    Task<Result<List<ClientResponse>>> FindAll(ClientFilterQuery query);
}

public class ClientService(IClientRepository repository) : IClientService
{
    private readonly IClientRepository m_ClientRepository = repository;

    public async Task<Result<List<ClientResponse>>> FindAll(ClientFilterQuery query)
    {
        var clients = await m_ClientRepository.FindAll(query);

        if (clients == null)
            return Result.NotFound<List<ClientResponse>>();

        var clientResponses = clients.Select(client => client.ToResponse())
                                     .ToList();

        return Result.Ok(clientResponses);
    }
}
