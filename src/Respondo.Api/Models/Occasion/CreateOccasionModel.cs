using System.ComponentModel.DataAnnotations;
using Respondo.Core.Occasions.Contracts;

namespace Respondo.Api.Models.Occasion;

/// <summary>
///     Model for creating an occasion.
/// </summary>
public record CreateOccasionModel
{
    /// <summary>
    ///     The name of the occasion.
    /// </summary>
    [Required]
    public required string Name { get; init; }
}

/// <summary>
///     Extensions for <see cref="CreateOccasionModel"/>.
/// </summary>
public static class CreateOccasionModelExtensions
{
    /// <summary>
    ///     Maps <see cref="CreateOccasionModel"/> to <see cref="CreateOccasion"/>.
    /// </summary>
    /// <param name="model">The <see cref="CreateOccasionModel"/> to map.</param>
    /// <param name="profileId">The id of the profile.</param>
    /// <returns>An instance of <see cref="CreateOccasion"/>.</returns>
    public static CreateOccasion ToRequest(this CreateOccasionModel model, Guid profileId)
    {
        return new CreateOccasion
        {
            Name = model.Name,
            ProfileId = profileId
        };
    }
}