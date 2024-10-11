namespace Respondo.Core.Events.Contracts;

public sealed record CreateEventResponse
{
    public required Guid Id { get; init; }
}