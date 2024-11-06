using System.ComponentModel.DataAnnotations;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Models.Party;

public record UpdatePartyModel
{
    /// <summary>
    ///     Name of the party.
    /// </summary>
    [MinLength(1), MaxLength(50)]
    public string? Name { get; init; }
    
    /// <summary>
    ///     Email address of the head of the party. Optional.
    /// </summary>
    [EmailAddress, MinLength(10), MaxLength(70)]
    public string? Email { get; init; }
}

/// <summary>
///     Extensions for <see cref="CreatePartyModel"/>.
/// </summary>
public static class UpdatePartyModelExtensions
{
    /// <summary>
    ///     Maps <see cref="UpdatePartyModel"/> to <see cref="UpdateParty"/>.
    /// </summary>
    /// <param name="model">The <see cref="CreatePartyModel"/> to map.</param>
    /// <param name="occasionId">The id of the occasion.</param>
    /// <param name="partyId">The id of the party.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="CreateParty"/>.</returns>
    public static UpdateParty ToRequest(this UpdatePartyModel model, Guid occasionId, Guid partyId, Guid profileId)
    {
        return new UpdateParty
        {
            Name = model.Name,
            Email = model.Email,
            PartyId = partyId,
            OccasionId = occasionId,
            ProfileId = profileId
        };
    }
}