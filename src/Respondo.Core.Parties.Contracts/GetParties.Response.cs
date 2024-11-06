namespace Respondo.Core.Parties.Contracts;

public sealed record GetPartiesResponse
{
    public required List<Party> Parties { get; init; }
    
    /// <summary>
    ///     Represents a party.
    /// </summary>
    public sealed record Party
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public string? Email { get; init; }
        public required IEnumerable<Member> Members { get; init; }

        /// <summary>
        ///     Represents a party member.
        /// </summary>
        public sealed record Member
        {
            public required Guid Id { get; init; }
            public required string Name { get; init; }
        }
    }
}