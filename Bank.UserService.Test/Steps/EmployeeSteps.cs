using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Services;
using Bank.UserService.Test.Examples.Entities;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace Bank.UserService.Test.Steps;

[Binding]
public class EmployeeSteps(ScenarioContext scenarioContext, IEmployeeService employeeService)
{
    private readonly IEmployeeService m_EmployeeService = employeeService;
    private readonly ScenarioContext  m_ScenarioContext = scenarioContext;

    [Given(@"employee create request")]
    public void GivenEmployeeCreateRequest()
    {
        m_ScenarioContext[Constant.CreateRequest] = Example.Entity.Employee.CreateRequest;
    }

    [When(@"employee is created in the database")]
    public async Task WhenEmployeeIsCreatedInTheDatabase()
    {
        var employeeCreateRequest = m_ScenarioContext.Get<EmployeeCreateRequest>(Constant.CreateRequest);

        var createEmployeeResult = await m_EmployeeService.Create(employeeCreateRequest);

        m_ScenarioContext[Constant.CreateResult] = createEmployeeResult;
    }

    [When(@"employee is fetched by Id")]
    public async Task WhenEmployeeIsFetchedById()
    {
        var createEmployeeResult = m_ScenarioContext.Get<Result<EmployeeResponse>>(Constant.CreateResult);

        var getEmployeeResult = createEmployeeResult.Value != null ? await m_EmployeeService.GetOne(createEmployeeResult.Value.Id) : Result.BadRequest<EmployeeResponse>();

        m_ScenarioContext[Constant.GetResult] = getEmployeeResult;
    }

    [Then(@"employee details should match the created employee")]
    public void ThenEmployeeDetailsShouldMatchTheCreatedEmployee()
    {
        var getEmployeeResult = m_ScenarioContext.Get<Result<EmployeeResponse>>(Constant.GetResult);

        getEmployeeResult.ActionResult.ShouldBeOfType<OkObjectResult>();
        getEmployeeResult.Value.ShouldNotBeNull();
        getEmployeeResult.Value.FirstName.ShouldBe(Example.Entity.Employee.CreateRequest.FirstName);
        getEmployeeResult.Value.LastName.ShouldBe(Example.Entity.Employee.CreateRequest.LastName);
        getEmployeeResult.Value.DateOfBirth.ShouldBe(Example.Entity.Employee.CreateRequest.DateOfBirth);
        getEmployeeResult.Value.Gender.ShouldBe(Example.Entity.Employee.CreateRequest.Gender);
        getEmployeeResult.Value.UniqueIdentificationNumber.ShouldBe(Example.Entity.Employee.CreateRequest.UniqueIdentificationNumber);
        getEmployeeResult.Value.Username.ShouldBe(Example.Entity.Employee.CreateRequest.Username);
        getEmployeeResult.Value.Email.ShouldBe(Example.Entity.Employee.CreateRequest.Email);
        getEmployeeResult.Value.PhoneNumber.ShouldBe(Example.Entity.Employee.CreateRequest.PhoneNumber);
        getEmployeeResult.Value.Address.ShouldBe(Example.Entity.Employee.CreateRequest.Address);
        getEmployeeResult.Value.Role.ShouldBe(Example.Entity.Employee.CreateRequest.Role);
        getEmployeeResult.Value.Department.ShouldBe(Example.Entity.Employee.CreateRequest.Department);
        getEmployeeResult.Value.Employed.ShouldBe(Example.Entity.Employee.CreateRequest.Employed);
    }
}

file static class Constant
{
    public const string CreateRequest = "EmployeeCreateRequest";
    public const string CreateResult  = "EmployeeCreateResult";
    public const string GetResult     = "EmployeeGetResult";
}
