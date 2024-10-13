namespace Respondo.Core.Occasions.Contracts;

public sealed record GetOccasionResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}