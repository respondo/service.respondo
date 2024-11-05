namespace Respondo.Core.Parties.Contracts;

public record UpdatePartyMember
{
    public required Guid PartyId { get; init; }
    public required Guid MemberId { get; init; }
    public required string Name { get; init; }
    public required Guid ProfileId { get; init; }
}