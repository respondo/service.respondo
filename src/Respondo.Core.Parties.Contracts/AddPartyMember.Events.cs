namespace Respondo.Core.Parties.Contracts;

public sealed record PartyMemberAdded
{
    public required Guid Id { get; init; }
}