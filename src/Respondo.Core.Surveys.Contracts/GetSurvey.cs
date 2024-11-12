namespace Respondo.Core.Surveys.Contracts;

public sealed record GetSurvey
{
    public required Guid SurveyId { get; init; }
}