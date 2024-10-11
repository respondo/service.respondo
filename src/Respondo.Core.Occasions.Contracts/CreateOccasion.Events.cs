namespace Respondo.Core.Occasions.Contracts;

public sealed record OccasionCreated
{
    public required Guid Id { get; init; }
}