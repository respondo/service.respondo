namespace Respondo.Core.Identity;

public sealed record CreateUser
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}