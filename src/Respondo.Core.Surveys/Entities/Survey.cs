namespace Respondo.Core.Surveys.Entities;

public class Survey
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Title { get; set; }
    
    public DateTimeOffset CreatedAt { get; init; } = TimeProvider.System.GetUtcNow();
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public List<Question> Questions { get; init; } = [];
    
    // External entity reference
    public required Guid OccasionId { get; set; }
    
    // External entity reference
    public required Guid ProfileId { get; set; }
}