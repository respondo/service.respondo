namespace Respondo.Core.Parties.Contracts;

public sealed record PartyMemberDeleted
{
    public required Guid Id { get; init; }
}