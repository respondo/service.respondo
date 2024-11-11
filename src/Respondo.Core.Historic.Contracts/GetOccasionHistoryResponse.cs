namespace Respondo.Core.Historic.Contracts;

public sealed record GetOccasionHistoryResponse
{
    public required Guid OccasionId { get; init; }
    public required List<Event> History { get; init; }

    public sealed record Event
    {        
        public required string Log { get; init; }
        public required DateTimeOffset Timestamp { get; init; }
    }
}