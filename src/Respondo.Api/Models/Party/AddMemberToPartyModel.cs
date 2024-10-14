using System.ComponentModel.DataAnnotations;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Models.Party;

public record AddMemberToPartyModel
{
    /// <summary>
    ///     The name of the member.
    /// </summary>
    [Required]
    public required string Name { get; init; }
}

/// <summary>
///     Extensions for <see cref="AddMemberToPartyModel"/>.
/// </summary>
public static class AddMemberToPartyModelExtensions
{
    /// <summary>
    ///     Maps <see cref="AddMemberToPartyModel"/> to <see cref="AddMemberToParty"/>.
    /// </summary>
    /// <param name="model">The <see cref="AddMemberToPartyModel"/> to map.</param>
    /// <param name="partyId">The id of the party.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="AddMemberToParty"/>.</returns>
    public static AddMemberToParty ToRequest(this AddMemberToPartyModel model, Guid partyId, Guid profileId)
    {
        return new AddMemberToParty
        {
            ProfileId = profileId,
            PartyId = partyId,
            Name = model.Name
        };
    }
}