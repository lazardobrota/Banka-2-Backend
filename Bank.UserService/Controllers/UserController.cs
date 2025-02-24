using Bank.Application.Endpoints;
using Bank.Application.Requests;
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

    [HttpPost(Endpoints.User.Login)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var result = await m_UserService.Login(userLoginRequest);

        return result.ActionResult;
    }
}
