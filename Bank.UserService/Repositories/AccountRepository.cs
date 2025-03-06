using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface IAccountRepository
{
    public Task<IEnumerable<Account>> FindAll(Guid userId); //todo 

    public Task<Account?> FindById(Guid id);
}

public class AccountRepository(ApplicationContext context) : IAccountRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<IEnumerable<Account>> FindAll(Guid userId)
    {
        return await m_Context.Accounts.Where(a => a.UserId == userId)
                              .ToListAsync();
    }

    public async Task<Account?> FindById(Guid id)
    {
        return await m_Context.Accounts.Where(a => a.Id == id)
                              .FirstOrDefaultAsync();
    }
}
