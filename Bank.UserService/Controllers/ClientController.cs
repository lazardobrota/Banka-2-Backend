using Bank.Application.Endpoints;
using Bank.UserService.Services;

using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace;

[ApiController]
public class ClientController(IClientService clientService) : ControllerBase
{
    private readonly IClientService m_clientService = clientService;

    [HttpGet(Endpoints.Client.GetAll)]
    public async Task<IActionResult> GetAll([FromBody] ClientFilterQuery filterQuery)
    {
        var result = await m_clientService.FindAll(filterQuery);

        return result.ActionResult;
    }
}
