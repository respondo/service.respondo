namespace Respondo.Core.Occasions.Contracts;

public sealed record OccasionCreated
{
    public required Guid OccasionId { get; init; }
    public required string Name { get; init; }
    public required Guid ProfileId { get; init; }
}