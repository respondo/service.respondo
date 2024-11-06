namespace Respondo.Core.Parties.Contracts;

public sealed record GetPartyResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
    public required List<Member> Members { get; init; }

    public sealed record Member
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}