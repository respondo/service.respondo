namespace Respondo.Core.Parties.Contracts;

public record AddPartyMember
{
    public required Guid PartyId { get; init; }
    public required string Name { get; init; }
    public required Guid ProfileId { get; init; }
}