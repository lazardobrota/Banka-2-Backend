using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IAccountService
{
    Task<Result<Page<AccountResponse>>> GetAll(AccountFilterQuery accountFilterQuery, Pageable pageable);

    Task<Result<AccountResponse>> GetOne(Guid id);
}

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    private readonly IAccountRepository m_AccountRepository = accountRepository;

    public async Task<Result<Page<AccountResponse>>> GetAll(AccountFilterQuery accountFilterQuery, Pageable pageable)
    {
        var page = await m_AccountRepository.FindAll(accountFilterQuery, pageable);

        var accountResponses = page.Items.Select(account => account.ToResponse())
                                   .ToList();

        return Result.Ok(new Page<AccountResponse>(accountResponses, page.PageNumber, page.PageSize, page.TotalElements));
    }

    public async Task<Result<AccountResponse>> GetOne(Guid id)
    {
        var account = await m_AccountRepository.FindById(id);

        if (account is null)
            return Result.NotFound<AccountResponse>($"No Account found with Id: {id}");

        return Result.Ok(account.ToResponse());
    }
}
