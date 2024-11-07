namespace Respondo.Core.Surveys.Contracts;

public sealed record DeleteQuestion
{
    public required Guid SurveyId { get; init; }
    public required Guid QuestionId { get; init; }
    public required Guid ProfileId { get; init; }
}