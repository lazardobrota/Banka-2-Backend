using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bank.UserService.Controllers;

[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService m_UserService = userService;

    [HttpGet(Endpoints.User.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilterQuery userFilterQuery, [FromQuery] Pageable pageable)
    {
        var result = await m_UserService.GetAll(userFilterQuery, pageable);

        return result.ActionResult;
    }

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

    [HttpPost(Endpoints.User.Activate)]
    public async Task<IActionResult> Activate([FromBody] UserActivationRequest userActivationRequest, [FromQuery] string token)
    {
        var result = await m_UserService.Activate(userActivationRequest, token);

        return result.ActionResult;
    }

    [HttpPost(Endpoints.User.RequestPasswordReset)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] UserRequestPasswordResetRequest passwordResetRequest)
    {
        var result = await m_UserService.RequestPasswordReset(passwordResetRequest);

        return result.ActionResult;
    }

    [HttpPost(Endpoints.User.PasswordReset)]
    public async Task<IActionResult> PasswordReset([FromBody] UserPasswordResetRequest userPasswordResetRequest, [FromQuery] string token)
    {
        var result = await m_UserService.PasswordReset(userPasswordResetRequest, token);

        return result.ActionResult;
    }
}
