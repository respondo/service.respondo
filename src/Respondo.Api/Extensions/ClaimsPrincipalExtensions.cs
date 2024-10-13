using System.Security.Claims;

namespace Respondo.Api.Extensions;

/// <summary>
///     Extensions for <see cref="ClaimsPrincipal"/>.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    ///     Get the profile id from the claims.
    /// </summary>
    /// <param name="user">The <see cref="ClaimsPrincipal"/> to get the profile id from.</param>
    /// <returns>The profile id.</returns>
    public static Guid GetProfileId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}