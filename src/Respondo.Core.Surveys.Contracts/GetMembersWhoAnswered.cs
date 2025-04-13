namespace Respondo.Core.Surveys.Contracts;

public record GetMembersWhoAnswered
{
    public required Guid SurveyId { get; init; }
}