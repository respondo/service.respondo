namespace Respondo.Core.Parties.Contracts;

public sealed record PartyMemberUpdated
{
    public required Guid Id { get; init; }
}