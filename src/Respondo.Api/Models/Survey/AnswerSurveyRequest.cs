using Respondo.Core.Surveys.Contracts;

namespace Respondo.Api.Models.Survey;

public sealed record AnswerSurveyRequest
{
    public required Guid SurveyId { get; init; }

    public required List<MemberAnswers> AnswersByMember { get; init; }

    public sealed record MemberAnswers
    {
        public required Guid MemberId { get; init; }

        public required List<QuestionAnswer> Answers { get; init; }
    }

    public sealed record QuestionAnswer
    {
        public required Guid QuestionId { get; init; }

        public string? Answer { get; init; }
    }
}

public static class AnswerSurveyRequestExtensions
{
    public static AnswerSurvey ToRequest(this AnswerSurveyRequest model, Guid partyId)
    {
        return new AnswerSurvey
        {
            SurveyId = model.SurveyId,
            PartyId = partyId,
            AnswersByMember = model.AnswersByMember
                .Select(member => (
                    member.MemberId,
                    member.Answers.Select(answer => (answer.QuestionId, answer.Answer)).ToArray()
                ))
                .ToList()
        };
    }
}
