namespace Respondo.Core.Parties.Contracts;

public sealed record PartyUpdated
{
    public required Guid Id { get; init; }
}