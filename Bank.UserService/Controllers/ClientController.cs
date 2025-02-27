using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Mvc;

namespace Bank.UserService.Controllers;

[ApiController]
public class ClientController(IClientService clientService) : ControllerBase
{
    private readonly IClientService m_clientService = clientService;

    [HttpGet(Endpoints.Client.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilterQuery filterQuery, [FromQuery] Pageable pageable)
    {
        filterQuery.Role = Role.Client;
        var result = await m_clientService.FindAll(filterQuery, pageable);

        return result.ActionResult;
    }

    [HttpGet(Endpoints.Client.GetOne)]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var result = await m_clientService.GetOne(id);

        return result.ActionResult;
    }

    [HttpPost(Endpoints.Client.Create)]
    public async Task<IActionResult> Create([FromBody] ClientCreateRequest clientCreateRequest)
    {
        var result = await m_clientService.Create(clientCreateRequest);

        return result.ActionResult;
    }

    [HttpPut(Endpoints.Client.Update)]
    public async Task<IActionResult> Update([FromBody] ClientUpdateRequest clientUpdateRequest, [FromRoute] Guid id)
    {
        var result = await m_clientService.Update(clientUpdateRequest, id);

        return result.ActionResult;
    }
}
