using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace DefaultNamespace;

public interface IClientRepository
{
    Task<List<Client>> FindAll(ClientFilterQuery clientFilterQuery);

    // Task<User?> FindById(Guid id);
    //
    // Task<User?> FindByEmail(string email);
    //
    // Task<User> Add(User user);
}

public class ClientRepository(ApplicationContext context) : IClientRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<List<Client>> FindAll(ClientFilterQuery clientFilterQuery)
    {
        var clientQuery = m_Context.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(clientFilterQuery.FirstName))
            clientQuery = clientQuery.Where(user => EF.Functions.ILike(user.FirstName, $"%{clientFilterQuery.FirstName}%"));

        if (!string.IsNullOrEmpty(clientFilterQuery.LastName))
            clientQuery = clientQuery.Where(user => EF.Functions.ILike(user.LastName, $"%{clientFilterQuery.LastName}%"));

        if (!string.IsNullOrEmpty(clientFilterQuery.Email))
            clientQuery = clientQuery.Where(user => EF.Functions.ILike(user.Email, $"%{clientFilterQuery.Email}%"));

        if (clientFilterQuery.Role != Role.Invalid)
            clientQuery = clientQuery.Where(user => user.Role == clientFilterQuery.Role);

        return await clientQuery.Skip((clientFilterQuery.Pagable.Page - 1) * clientFilterQuery.Pagable.Size)
                                .Take(clientFilterQuery.Pagable.Size)
                                .ToListAsync();
    }
}
