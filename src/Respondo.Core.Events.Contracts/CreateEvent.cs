namespace Respondo.Core.Events.Contracts;

public sealed record CreateEvent
{
    public required string Name { get; init; }
    public required Guid ProfileId { get; init; }
}