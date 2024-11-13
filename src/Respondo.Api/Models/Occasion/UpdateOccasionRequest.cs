using System.ComponentModel.DataAnnotations;
using Respondo.Core.Occasions.Contracts;

namespace Respondo.Api.Models.Occasion;

/// <summary>
///     Model for updating an occasion.
/// </summary>
public record UpdateOccasionRequest
{
    /// <summary>
    ///     The new name of the occasion. Optional.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    ///     The new base url of the occasion. Optional.
    /// </summary>
    public string? BaseUrl { get; init; }
    
    /// <summary>
    ///     The new logo of the occasion. Optional.
    /// </summary>
    public string? Logo { get; init; }
}

/// <summary>
///     Extensions for <see cref="UpdateOccasionRequest"/>.
/// </summary>
public static class UpdateOccasionRequestExtensions
{
    public static UpdateOccasion ToRequest(this UpdateOccasionRequest model, Guid occasionId, Guid profileId)
    {
        return new UpdateOccasion
        {
            OccasionId = occasionId,
            ProfileId = profileId,
            Name = model.Name,
            BaseUrl = model.BaseUrl,
            Logo = model.Logo
        };
    }
}