using Bank.Application;
using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.UserService.Controllers;

[ApiController]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet(Endpoints.Employee.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilterQuery userFilterQuery, [FromQuery] Pageable pageable)
    {
        userFilterQuery.Role = Role.Employee;
        var result = await employeeService.GetAll(userFilterQuery, pageable);

        return result.ActionResult;
    }
    
    [HttpGet(Endpoints.Employee.GetOne)]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var result = await employeeService.GetOne(id);

        return result.ActionResult;
    }

    [HttpPost(Endpoints.Employee.Create)]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest employeeCreateRequest)
    {
        var result = await employeeService.Create(employeeCreateRequest);

        return result.ActionResult;
    }

    [HttpPut(Endpoints.Employee.Update)]
    public async Task<IActionResult> Update([FromBody] EmployeeUpdateRequest employeeUpdateRequest, [FromRoute] Guid id)
    {
        var result = await employeeService.Update(employeeUpdateRequest, id);

        return result.ActionResult;
    }
}
