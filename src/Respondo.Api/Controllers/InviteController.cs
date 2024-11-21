using Microsoft.AspNetCore.Mvc;
using Respondo.Core.Parties.Contracts;
using Wolverine;

namespace Respondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InviteController : ControllerBase
{
    private readonly IMessageBus _bus;

    public InviteController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet("{partyId:guid}/party")]
    public async Task<IActionResult> GetParty([FromRoute] Guid partyId, CancellationToken cancellationToken)
    {
        var request = new GetParty { Id = partyId };
        
        var response = await _bus.InvokeAsync<GetPartyResponse?>(request, cancellationToken);
        
        if (response is null)
        {
            return NotFound();
        }
        
        return Ok(response);
    }
    
    [HttpGet("{partyId:guid}survey")]
    public Task<IActionResult> GetSurveyByParty([FromRoute] Guid partyId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}