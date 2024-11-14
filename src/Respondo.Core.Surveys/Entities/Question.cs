namespace Respondo.Core.Surveys.Entities;

public abstract class Question
{
    public Question()
    {
        Answers = [];
    }

    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Statement { get; init; }
    public required bool Required { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = TimeProvider.System.GetUtcNow();
    
    public List<Answer> Answers { get; init; }
    public required Survey Survey { get; init; }
}

public class OpenQuestion : Question { }

public class GeneralQuestion : Question { }

public class SingleChoiceQuestion : Question
{
    public required List<string> Options { get; init; } = [];
}

public class MultipleChoiceQuestion : Question
{
    public required List<string> Options { get; init; } = [];
}