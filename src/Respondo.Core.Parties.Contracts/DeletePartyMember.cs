namespace Respondo.Core.Parties.Contracts;

public sealed record DeletePartyMember
{
    public required Guid PartyId { get; init; }
    public required Guid Id { get; init; }
    public required Guid ProfileId { get; init; }
}