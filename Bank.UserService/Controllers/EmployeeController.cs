using Bank.Application.Endpoints;
using Bank.Application.Requests;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bank.UserService.Controllers;

[ApiController]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost(Endpoints.Employee.Create)]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest employeeCreateRequest)
    {
        var result = await employeeService.Create(employeeCreateRequest);

        return result.ActionResult;
    }
}
