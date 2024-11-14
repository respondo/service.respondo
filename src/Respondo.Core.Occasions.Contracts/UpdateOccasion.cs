namespace Respondo.Core.Occasions.Contracts;

public sealed record UpdateOccasion
{
    public required Guid OccasionId { get; init; }
    public required Guid ProfileId { get; init; }
    public string? Name { get; init; }
    public string? BaseUrl { get; init; }
    public string? Logo { get; init; }
}