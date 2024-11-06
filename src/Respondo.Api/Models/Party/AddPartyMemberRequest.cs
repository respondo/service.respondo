using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Models.Party;

public record AddPartyMemberRequest
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
public static class AddPartyMemberRequestExtensions
{
    /// <summary>
    ///     Maps <see cref="AddPartyMemberRequest"/> to <see cref="AddPartyMember"/>.
    /// </summary>
    /// <param name="model">The <see cref="AddPartyMemberRequest"/> to map.</param>
    /// <param name="partyId">The id of the party.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="AddPartyMember"/>.</returns>
    public static AddPartyMember ToRequest(this AddPartyMemberRequest model, Guid partyId, Guid profileId)
    {
        return new AddPartyMember
        {
            ProfileId = profileId,
            PartyId = partyId,
            Name = model.Name
        };
    }
}