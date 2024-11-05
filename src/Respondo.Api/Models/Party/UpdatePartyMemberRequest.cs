using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Models.Party;

public record UpdatePartyMemberRequest
{
    /// <summary>
    ///     The name of the member.
    /// </summary>
    [Required]
    [Description("The name of the member.")]
    public required string Name { get; init; }
}

/// <summary>
///     Extensions for <see cref="AddPartyMemberRequest"/>.
/// </summary>
public static class UpdatePartyMemberRequestExtensions
{
    /// <summary>
    ///     Maps <see cref="UpdatePartyMemberRequest"/> to <see cref="UpdatePartyMember"/>.
    /// </summary>
    /// <param name="model">The <see cref="AddPartyMemberRequest"/> to map.</param>
    /// <param name="partyId">The id of the party.</param>
    /// <param name="memberId">The id of the member.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="AddPartyMember"/>.</returns>
    public static UpdatePartyMember ToRequest(this UpdatePartyMemberRequest model, Guid partyId, Guid memberId, Guid profileId)
    {
        return new UpdatePartyMember
        {
            ProfileId = profileId,
            PartyId = partyId,
            MemberId = memberId,
            Name = model.Name
        };
    }
}