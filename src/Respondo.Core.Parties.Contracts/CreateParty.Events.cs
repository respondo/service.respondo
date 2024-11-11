namespace Respondo.Core.Parties.Contracts;

public sealed record PartyCreated
{
    public required Guid PartyId { get; init; }
    public required string Name { get; init; }
    public required Guid OccasionId { get; init; }
}