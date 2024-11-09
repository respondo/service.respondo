namespace Respondo.Core.Surveys.Entities;

public class Answer
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string? Value { get; init; }
    
    public DateTimeOffset CreatedAt { get; init; } = TimeProvider.System.GetUtcNow();
    
    public required Question Question { get; init; }
    
    // External entity reference
    public required Guid MemberId { get; init; }
}