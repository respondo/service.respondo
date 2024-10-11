using System.ComponentModel.DataAnnotations;
using Respondo.Core.Identity.Contracts;

namespace Respondo.Api.Models;

/// <summary>
///     Model for registering a new user.
/// </summary>
public record RegisterModel
{
    /// <summary>
    ///     Username for the user.
    /// </summary>
    [Required]
    public required string Username { get; init; }
    
    /// <summary>
    ///     Email for the user.
    /// </summary>
    [Required, EmailAddress]
    public required string Email { get; init; }
    
    /// <summary>
    ///     Password for the user.
    /// </summary>
    [Required]
    public required string Password { get; init; }
}

/// <summary>
///     Extensions for <see cref="RegisterModel"/>.
/// </summary>
internal static class RegisterModelExtensions
{
    /// <summary>
    ///     Maps <see cref="RegisterModel"/> to <see cref="CreateApplicationUser"/>
    /// </summary>
    /// <param name="model">The <see cref="RegisterModel"/> to map.</param>
    /// <returns>An instance of <see cref="CreateApplicationUser"/>.</returns>
    public static CreateApplicationUser ToRequest(this RegisterModel model)
    {
        return new CreateApplicationUser
        {
            Username = model.Username,
            Email = model.Email,
            Password = model.Password
        };
    }
}