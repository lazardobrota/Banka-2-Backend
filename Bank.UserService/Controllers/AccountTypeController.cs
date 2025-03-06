using Bank.Application.Endpoints;
using Bank.Application.Responses;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Role = Bank.UserService.Configurations.Configuration.Policy.Role;

namespace Bank.UserService.Controllers;

[ApiController]
public class AccountTypeController(IAccountTypeService accountTypeService) : ControllerBase
{
    private readonly IAccountTypeService m_AccountTypeService = accountTypeService;

    [HttpGet(Endpoints.AccountType.GetOne)]
    [Authorize(Roles = $"{Role.Admin}, {Role.Employee}")]
    public async Task<ActionResult<AccountTypeResponse>> GetOne([FromQuery] Guid id)
    {
        var result = await m_AccountTypeService.GetOne(id);

        return result.ActionResult;
    }
}
