using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

using Result = Bank.Application.Endpoints.Result;

namespace Bank.UserService.Services;

public interface IAccountTypeService
{
    Task<Result<AccountTypeResponse>> GetOne(Guid id);
}

public class AccountTypeService(IAccountTypeRepository accountTypeRepository) : IAccountTypeService
{
    private readonly IAccountTypeRepository m_AccountTypeRepository = accountTypeRepository;

    public async Task<Result<AccountTypeResponse>> GetOne(Guid id)
    {
        var accountType = await m_AccountTypeRepository.FindById(id);

        if (accountType is null)
            return Result.NotFound<AccountTypeResponse>($"No Account type found with Id: {id}");

        return Result.Ok(accountType.ToResponse());
    }
}
