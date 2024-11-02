namespace Respondo.Core.Occasions.Contracts;

public sealed record GetOccasions
{
    public required Guid ProfileId { get; init; }
}