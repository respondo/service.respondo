namespace Respondo.Core.Historic.Aggregates;

public sealed record OccasionHistory
{
    public required Guid Id { get; init; }

    public List<Event> History { get; init; } = [];

    public sealed record Event
    {
        public required string Log { get; init; }
        public required DateTimeOffset Timestamp { get; init; }
    }
}