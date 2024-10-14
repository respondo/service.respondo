namespace Respondo.Core.Parties.Contracts;

public sealed record GetParty
{
    public required Guid Id { get; init; }
    public required Guid ProfileId { get; init; }
}