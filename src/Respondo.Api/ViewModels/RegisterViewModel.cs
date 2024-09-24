namespace Respondo.Api.ViewModels;

public record RegisterViewModel
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}