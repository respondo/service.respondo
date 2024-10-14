namespace Respondo.Core.Parties.Contracts;

public sealed record MemberAddedToParty
{
    public required Guid Id { get; init; }
}