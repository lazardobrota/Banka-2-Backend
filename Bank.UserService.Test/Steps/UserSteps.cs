using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Services;
using Bank.UserService.Test.Examples.Entities;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace Bank.UserService.Test.Steps;

[Binding]
public class UserSteps(IUserService userService, ScenarioContext scenarioContext)
{
    private readonly IUserService    m_UserService     = userService;
    private readonly ScenarioContext m_ScenarioContext = scenarioContext;

    [Given(@"user login request")]
    public void GivenUserLoginRequest()
    {
        m_ScenarioContext[Constant.LoginRequest] = Example.Entity.User.LoginRequest;
    }

    [When(@"user sends valid login request")]
    public async Task WhenUserSendsValidLoginRequest()
    {
        var userLoginRequest = m_ScenarioContext.Get<UserLoginRequest>(Constant.LoginRequest);

        var userLoginResult = await m_UserService.Login(userLoginRequest);

        m_ScenarioContext[Constant.LoginResult] = userLoginResult;
    }

    [Then(@"login should be successful")]
    public void ThenLoginShouldBeSuccessful()
    {
        var userLoginResult = m_ScenarioContext.Get<Result<UserLoginResponse>>(Constant.LoginResult);

        userLoginResult.ActionResult.ShouldBeOfType<OkObjectResult>();
    }

    [Then(@"user should get valid jwt")]
    public void ThenUserShouldGetValidJwt()
    {
        var userLoginResult = m_ScenarioContext.Get<Result<UserLoginResponse>>(Constant.LoginResult);

        userLoginResult.Value.ShouldNotBeNull();
        userLoginResult.Value.Token.ShouldNotBeEmpty();
    }
}

file static class Constant
{
    public const string LoginRequest = "EmployeeCreateRequest";
    public const string LoginResult  = "EmployeeCreateResult";
}
