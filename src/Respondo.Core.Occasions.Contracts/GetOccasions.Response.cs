namespace Respondo.Core.Occasions.Contracts;

public sealed record GetOccasionsResponse
{
    public required List<Occasion> Occasions { get; init; }
    
    public sealed record Occasion
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }
}