using System.ComponentModel.DataAnnotations;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Api.Models.Party;

public record CreatePartyModel
{
    /// <summary>
    ///     Name of the party.
    /// </summary>
    [Required]
    public required string Name { get; init; }
    
    /// <summary>
    ///     Email address of the head of the party. Optional.
    /// </summary>
    [EmailAddress]
    public string? Email { get; init; }
}

/// <summary>
///     Extensions for <see cref="CreatePartyModel"/>.
/// </summary>
public static class CreatePartyModelExtensions
{
    /// <summary>
    ///     Maps <see cref="CreatePartyModel"/> to <see cref="CreateParty"/>.
    /// </summary>
    /// <param name="model">The <see cref="CreatePartyModel"/> to map.</param>
    /// <param name="occasionId">The id of the occasion.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="CreateParty"/>.</returns>
    public static CreateParty ToRequest(this CreatePartyModel model, Guid occasionId, Guid profileId)
    {
        return new CreateParty
        {
            Name = model.Name,
            Email = model.Email,
            OccasionId = occasionId,
            ProfileId = profileId
        };
    }
}