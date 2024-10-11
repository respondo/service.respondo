using NodaTime;

namespace Respondo.Core.Events.Contracts.Entities;

public class Event
{
    public Event()
    {
        CreatedAt = SystemClock.Instance.GetCurrentInstant();
        LastUpdatedAt = SystemClock.Instance.GetCurrentInstant();
    }
    
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required Profile Profile { get; set; }

    public Instant CreatedAt { get; set; }
    
    public Instant LastUpdatedAt { get; set; }
    
}