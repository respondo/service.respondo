namespace Respondo.Core.Parties.Contracts;

public sealed record CreatePartyResponse
{
    public required Guid Id { get; init; }
}