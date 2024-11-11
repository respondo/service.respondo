namespace Respondo.Core.Historic.Contracts;

public sealed record GetOccasionHistory
{
    public Guid OccasionId { get; init; }
}