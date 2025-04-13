namespace Respondo.Core.Surveys.Contracts;

public sealed record GetAnswersByPartyResponse
{
    public required Guid SurveyId { get; init; }
    public required string SurveyTitle { get; init; }
    public required IEnumerable<MemberAnswers> AnswersByMember { get; init; }

    public sealed record MemberAnswers
    {
        public required Guid MemberId { get; init; }
        public required IEnumerable<QuestionAnswer> Answers { get; init; }
    }

    public sealed record QuestionAnswer
    {
        public required Guid QuestionId { get; init; }
        public required string QuestionStatement { get; init; }
        public required string QuestionType { get; init; }
        public required string? Answer { get; init; }
    }
}
