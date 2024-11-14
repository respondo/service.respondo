namespace Respondo.Core.Occasions.Contracts;

public sealed record GetOccasionResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? BaseUrl { get; init; }
    public string? Logo { get; init; }
}