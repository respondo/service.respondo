using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Respondo.Core.Historic.Contracts;
using Wolverine;

namespace Respondo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HistoricController : ControllerBase
{
    private readonly IMessageBus _bus;

    public HistoricController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet("{occasionId:guid}")]
    public async Task<IActionResult> GetOccasionHistory([FromRoute] Guid occasionId,
        CancellationToken cancellationToken)
    {
        var query = new GetOccasionHistory
        {
            OccasionId = occasionId
        };
        
        var result = await _bus.InvokeAsync<GetOccasionHistoryResponse?>(query, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
}