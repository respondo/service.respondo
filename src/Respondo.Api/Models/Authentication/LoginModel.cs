using System.ComponentModel.DataAnnotations;

namespace Respondo.Api.Models.Authentication;

/// <summary>
///     Model for login an existing user.
/// </summary>
public class LoginModel
{
    /// <summary>
    ///     Email of the user.
    /// </summary>
    [Required, EmailAddress]
    public required string Email { get; init; }
    
    /// <summary>
    ///     Password of the user.
    /// </summary>
    [Required]
    public required string Password { get; init; }
}