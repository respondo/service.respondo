using System.ComponentModel.DataAnnotations;
using Respondo.Core.Surveys.Contracts;

namespace Respondo.Api.Models.Survey;

public sealed record AddQuestionRequest
{
    [Required, Range(0, 3)]
    public required EQuestionType Type { get; init; }
    
    [Required, Length(1, 200)]
    public required string Statement { get; init; }
    
    [Required]
    public required bool Required { get; init; }
    
    public List<string>? Options { get; init; }
    
    public enum EQuestionType
    {
        General,
        Open,
        SingleChoice,
        MultipleChoice
    }
}

public static class AddQuestionRequestExtensions
{
    public static AddQuestion ToRequest(this AddQuestionRequest model, Guid surveyId, Guid profileId)
    {
        return new AddQuestion
        {
            SurveyId = surveyId,
            Type = (AddQuestion.EQuestionType)model.Type,
            Statement = model.Statement,
            Required = model.Required,
            Options = model.Options,
            ProfileId = profileId
        };
    }
}