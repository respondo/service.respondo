namespace Respondo.Core.Historic.Aggregates;

public record Occasion()
{
    public required Guid Id { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
}