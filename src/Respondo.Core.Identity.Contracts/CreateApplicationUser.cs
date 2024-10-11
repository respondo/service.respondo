namespace Respondo.Core.Identity.Contracts;

public sealed record CreateApplicationUser
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}