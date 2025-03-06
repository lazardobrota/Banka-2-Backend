using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IAccountService
{
    Task<Result<AccountResponse>> GetOne(Guid id);
}

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    private readonly IAccountRepository m_AccountRepository = accountRepository;

    public async Task<Result<AccountResponse>> GetOne(Guid id)
    {
        var account = await m_AccountRepository.FindById(id);

        if (account is null)
            return Result.NotFound<AccountResponse>($"No Account found with Id: {id}");

        return Result.Ok(account.ToResponse());
    }
}
