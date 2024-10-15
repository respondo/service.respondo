namespace Respondo.Core.Parties.Contracts;

public sealed record DeletePartyMember
{
    public Guid PartyId { get; set; }
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
}