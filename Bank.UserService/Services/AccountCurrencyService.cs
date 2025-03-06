using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

namespace Bank.UserService.Services;

public interface IAccountCurrencyService
{
    Task<Result<AccountCurrencyResponse>> GetOne(Guid id);
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
}
