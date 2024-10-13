namespace Respondo.Core.Parties.Contracts;

public sealed record CreateParty
{
    public required Guid OccasionId { get; init; }
    public Guid ProfileId { get; init; }
    public required string Name { get; init; }
    public string? Email { get; init; }
}