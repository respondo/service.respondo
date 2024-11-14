namespace Respondo.Core.Surveys.Contracts;

public sealed record AddQuestion
{
    public required Guid SurveyId { get; init; }
    public required EQuestionType Type { get; init; }
    public required string Statement { get; init; }
    public required bool Required { get; init; }
    public List<string>? Options { get; init; }
    public required Guid ProfileId { get; init; }
    
    public enum EQuestionType
    {
        General,
        Open,
        SingleChoice,
        MultipleChoice
    }
}