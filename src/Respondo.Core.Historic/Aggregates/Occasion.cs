namespace Respondo.Core.Historic.Aggregates;

public sealed record Occasion()
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public  List<Party> Parties { get; init; } = [];
    
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }

    public sealed record Party()
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        
        public required DateTimeOffset CreatedAt { get; init; }
        public required DateTimeOffset UpdatedAt { get; init; }
    }
}