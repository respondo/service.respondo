namespace Respondo.Core.Surveys.Entities;

public abstract class Question
{
    public Question()
    {
        Answers = [];
    }

    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Statement { get; set; }
    public required bool Required { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = TimeProvider.System.GetUtcNow();
    
    public List<Answer> Answers { get; set; }
    public required Survey Survey { get; set; }
}

public class OpenQuestion : Question { }

public class GeneralQuestion : Question { }

public class SingleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = [];
}

public class MultipleChoiceQuestion : Question
{
    public List<string> Options { get; set; } = [];
}