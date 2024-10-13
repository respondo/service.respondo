namespace Respondo.Core.Parties.Contracts;

public sealed record PartyCreated
{
    public required Guid Id { get; init; }
}