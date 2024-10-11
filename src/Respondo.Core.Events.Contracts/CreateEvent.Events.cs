namespace Respondo.Core.Events.Contracts;

public sealed record EventCreated
{
    public required Guid Id { get; init; }
}