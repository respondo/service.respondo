namespace Respondo.Core.Surveys.Contracts;

public sealed record GetSurveyResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required IEnumerable<Question> Questions { get; set; }

    public record Question
    {
        public required Guid Id { get; init; }
        public required string Statement { get; init; }
        public required bool Required { get; init; }
        public List<string>? Options { get; init; }
    }
}