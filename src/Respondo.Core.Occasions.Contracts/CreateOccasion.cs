namespace Respondo.Core.Occasions.Contracts;

public sealed record CreateOccasion
{
    public required string Name { get; init; }
    public required Guid ProfileId { get; init; }
}