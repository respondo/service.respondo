using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Models.Survey;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Surveys.Contracts;
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

    [HttpGet("{partyId:guid}/survey")]
    public async Task<IActionResult> GetSurveyByParty([FromRoute] Guid partyId, CancellationToken cancellationToken)
    {
        var request = new GetSurveyByParty { PartyId = partyId };

        var response = await _bus.InvokeAsync<GetSurveyByPartyResponse?>(request, cancellationToken);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpGet("{partyId:guid}/answers")]
    public async Task<IActionResult> GetAnswersByParty([FromRoute] Guid partyId, CancellationToken cancellationToken)
    {
        var request = new GetAnswersByParty { PartyId = partyId };

        var response = await _bus.InvokeAsync<GetAnswersByPartyResponse?>(request, cancellationToken);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost("{partyId:guid}/survey/answer")]
    public async Task<IActionResult> AnswerSurvey([FromRoute] Guid partyId, [FromBody] AnswerSurveyRequest request)
    {
        var command = request.ToRequest(partyId);

        await _bus.SendAsync(command);

        return Accepted();
    }
}
