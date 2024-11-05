using Microsoft.Extensions.Internal;

namespace Respondo.Core.Occasions.Entities;

public class Occasion
{
    public Occasion()
    {
        CreatedAt = TimeProvider.System.GetUtcNow();
        LastUpdatedAt = TimeProvider.System.GetUtcNow();
    }
    
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required Profile Profile { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset LastUpdatedAt { get; set; }
    
}