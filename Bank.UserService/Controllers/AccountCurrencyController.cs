using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Role = Bank.UserService.Configurations.Configuration.Policy.Role;

namespace Bank.UserService.Controllers;

[ApiController]
public class AccountCurrencyController(IAccountCurrencyService accountCurrencyService) : ControllerBase
{
    private readonly IAccountCurrencyService m_AccountCurrencyService = accountCurrencyService;
    

    [Authorize]
    [HttpGet(Endpoints.AccountCurrency.GetOne)]
    public async Task<ActionResult<AccountCurrencyResponse>> GetOne([FromQuery] Guid id)
    {
        var result = await m_AccountCurrencyService.GetOne(id);

        return result.ActionResult;
    }
    
}
