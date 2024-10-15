using Microsoft.AspNetCore.Mvc;
using Respondo.Api.Extensions;
using Respondo.Api.Models.Party;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Controllers;

public partial class OccasionController
{
    [HttpPost("{occasionId:guid}/party/{partyId:guid}/member")]
    public async Task<IActionResult> AddPartyMember([FromRoute] Guid occasionId, [FromRoute] Guid partyId,
        [FromBody] AddPartyMemberRequest model)
    {
        var request = model.ToRequest(partyId, User.GetProfileId());

        await _bus.PublishAsync(request);

        return Accepted();
    }

    [HttpDelete("{occasionId:guid}/party/{partyId:guid}/member/{memberId:guid}")]
    public async Task<IActionResult> DeletePartyMember([FromRoute] Guid occasionId, [FromRoute] Guid partyId,
        [FromRoute] Guid memberId)
    {
        var request = new DeletePartyMember()
        {
            PartyId = partyId,
            Id = memberId,
            ProfileId = User.GetProfileId(),
        };

        await _bus.PublishAsync(request);

        return Accepted();
    }
}