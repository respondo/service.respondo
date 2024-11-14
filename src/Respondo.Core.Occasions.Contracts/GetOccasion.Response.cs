namespace Respondo.Core.Occasions.Contracts;

public sealed record GetOccasionResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? BaseUrl { get; init; }
    public required string? Logo { get; init; }
}