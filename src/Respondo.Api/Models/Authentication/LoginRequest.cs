using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Respondo.Api.Models.Authentication;

/// <summary>
///     Request for login an existing user.
/// </summary>
public class LoginRequest
{
    /// <summary>
    ///     Email of the user.
    /// </summary>
    [Required, EmailAddress]
    [Description("Email of the user.")]
    public required string Email { get; init; }
    
    /// <summary>
    ///     Password of the user.
    /// </summary>
    [Required]
    [Description("Password of the user.")]
    public required string Password { get; init; }
}