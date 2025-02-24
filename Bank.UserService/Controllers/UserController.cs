using Bank.Application.Endpoints;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bank.UserService.Controllers;

[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService m_UserService = userService;

    [HttpGet(Endpoints.User.GetOne)]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var result = await m_UserService.GetOne(id);

        return result.ActionResult;
    }
}
