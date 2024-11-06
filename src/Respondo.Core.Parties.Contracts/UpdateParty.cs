namespace Respondo.Core.Parties.Contracts;

public sealed record UpdateParty
{
    public required Guid OccasionId { get; init; }
    public required Guid PartyId { get; init; }
    public required Guid ProfileId { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
}