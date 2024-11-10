namespace Respondo.Core.Parties.Contracts;

public sealed record PartyUpdated
{
    public required Guid PartyId { get; init; }
    public required Guid OccasionId { get; init; }
}