using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Party;
using Respondo.Core.Parties.Contracts;
using Wolverine;

namespace Respondo.Api.Controllers;

public partial class OccasionController
{
    [HttpGet("{occasionId:guid}/party/{partyId:guid}")]
    public async Task<IActionResult> GetParty([FromRoute] Guid occasionId, [FromRoute] Guid partyId, CancellationToken cancellationToken)
    {
        var request = new GetParty
        {
            Id = partyId,
            ProfileId = User.GetProfileId()
        };

        var result = await _bus.InvokeAsync<GetPartyResponse?>(request, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("{occasionId:guid}/party")]
    public async Task<IActionResult> CreateParty([FromRoute] Guid occasionId, [FromBody] CreatePartyModel model,
        CancellationToken cancellationToken)
    {
        var request = model.ToRequest(occasionId, User.GetProfileId());

        var result = await _bus.InvokeAsync<CreatePartyResponse?>(request, cancellationToken);

        if (result is null)
        {
            return Problem("Unable to create party. Please contact support.");
        }

        return Ok(result);
    }

    [HttpDelete("{occasionId:guid}/party/{partyId:guid}")]
    public async Task<IActionResult> DeleteParty([FromRoute] Guid occasionId, [FromRoute] Guid partyId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteParty
        {
            Id = partyId,
            ProfileId = User.GetProfileId()
        };

        await _bus.InvokeAsync(request, cancellationToken);

        return Accepted();
    }
}