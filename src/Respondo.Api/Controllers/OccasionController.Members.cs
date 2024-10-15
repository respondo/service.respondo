using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Party;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Controllers;

public partial class OccasionController
{
    [HttpPost("{occasionId:guid}/party/{partyId:guid}/member")]
    public async Task<IActionResult> AddMemberToParty([FromRoute] Guid occasionId, [FromRoute] Guid partyId,
        [FromBody] AddMemberToPartyModel model)
    {
        var request = model.ToRequest(partyId, User.GetProfileId());

        await _bus.PublishAsync(request);

        return Accepted();
    }
}