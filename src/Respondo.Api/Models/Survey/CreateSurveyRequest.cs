using Respondo.Core.Surveys.Contracts;

namespace Respondo.Api.Models.Survey;

public record CreateSurveyRequest
{
    public required string Title { get; init; }
}

public static class CreateSurveyRequestExtensions
{
    public static CreateSurvey ToRequest(this CreateSurveyRequest model, Guid occasionId, Guid profileId)
    {
        return new CreateSurvey
        {
            OccasionId = occasionId,
            Title = model.Title,
            ProfileId = profileId
        };
    }
}