namespace Respondo.Core.Surveys.Contracts;

public sealed record AnswerSurvey
{
    public required Guid SurveyId { get; init; }
    public required Guid PartyId { get; init; }
    
    public required List<(Guid MemberId, (Guid QuestionId, string Answer)[])> AnswersByMember { get; init; }
}