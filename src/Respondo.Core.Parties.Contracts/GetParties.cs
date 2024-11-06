namespace Respondo.Core.Parties.Contracts;

public sealed record GetParties
{
    public required Guid OccasionId { get; init; }
    public required Guid ProfileId { get; init; }
}