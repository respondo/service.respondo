namespace Respondo.Core.Occasions.Contracts;

public sealed record CreateOccasionResponse
{
    public required Guid Id { get; init; }
}