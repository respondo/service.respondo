namespace Respondo.Core.Parties.Contracts;

public record CreateParty
{
    public required Guid OccasionId { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
}