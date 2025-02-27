using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Role = Bank.UserService.Configurations.Configuration.Policy.Role;

namespace Bank.UserService.Controllers;

[ApiController]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet(Endpoints.Employee.GetAll)]
    [Authorize(Roles = $"{Role.Admin}, {Role.Employee}")]
    public async Task<IActionResult> GetAll([FromQuery] UserFilterQuery userFilterQuery, [FromQuery] Pageable pageable)
    {
        var result = await employeeService.GetAll(userFilterQuery, pageable);

        return result.ActionResult;
    }

    [HttpGet(Endpoints.Employee.GetOne)]
    [Authorize(Roles = $"{Role.Admin}, {Role.Employee}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var result = await employeeService.GetOne(id);

        return result.ActionResult;
    }

    [HttpPost(Endpoints.Employee.Create)]
    [Authorize(Roles = $"{Role.Admin}, {Role.Employee}")]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest employeeCreateRequest)
    {
        var result = await employeeService.Create(employeeCreateRequest);

        return result.ActionResult;
    }

    [HttpPut(Endpoints.Employee.Update)]
    [Authorize(Roles = $"{Role.Admin}, {Role.Employee}")]
    public async Task<IActionResult> Update([FromBody] EmployeeUpdateRequest employeeUpdateRequest, [FromRoute] Guid id)
    {
        var result = await employeeService.Update(employeeUpdateRequest, id);

        return result.ActionResult;
    }
}
