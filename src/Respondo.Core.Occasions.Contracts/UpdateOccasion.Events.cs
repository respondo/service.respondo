namespace Respondo.Core.Occasions.Contracts;

public sealed record OccasionUpdated
{
    public required Guid OccasionId { get; init; }
}