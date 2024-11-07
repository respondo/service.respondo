namespace Respondo.Core.Surveys.Entities;

public class Answer
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string? Value { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = TimeProvider.System.GetUtcNow();
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public required Question Question { get; set; }
    public required Guid PartyId { get; set; }
}