using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IAccountCurrencyService
{
    Task<Result<AccountCurrencyResponse>>       GetOne(Guid     id);
    Task<Result<Page<AccountCurrencyResponse>>> GetAll(Pageable pageable);

}

public class AccountCurrencyService(IAccountCurrencyRepository accountCurrencyRepository) : IAccountCurrencyService
{
    private readonly IAccountCurrencyRepository m_AccountCurrencyRepository = accountCurrencyRepository;

    public async Task<Result<AccountCurrencyResponse>> GetOne(Guid id)
    {
        var accountCurrency = await m_AccountCurrencyRepository.FindById(id);

        if (accountCurrency == null)
            return Result.NotFound<AccountCurrencyResponse>($"No AccountCurrency found with Id: {id}");

        return Result.Ok(accountCurrency.ToResponse());
    }
    public async Task<Result<Page<AccountCurrencyResponse>>> GetAll(Pageable pageable)
    {
        var page = await m_AccountCurrencyRepository.FindAll(pageable);

        var accountCurrencyResponses = page.Items.Select(accountCurrency => accountCurrency.ToResponse())
                                           .ToList();

        return Result.Ok(new Page<AccountCurrencyResponse>(accountCurrencyResponses, page.PageNumber, page.PageSize, page.TotalElements));
    }
}
