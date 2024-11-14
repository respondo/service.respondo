using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Services;

public class AnswerValidationService : IAnswerValidationService
{
    public bool IsValid(Question question, string? answer)
    {
        return question switch
        {
            GeneralQuestion generalQuestion => IsValidGeneralQuestion(generalQuestion, answer),
            OpenQuestion openQuestion => IsValidOpenQuestion(openQuestion, answer),
            SingleChoiceQuestion singleChoiceQuestion => throw new NotImplementedException(),
            MultipleChoiceQuestion multipleChoiceQuestion => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(question))
        };
    }
    
    private bool IsValidGeneralQuestion(GeneralQuestion question, string? answer)
    {
        return question switch
        {
            { Required: false } when string.IsNullOrEmpty(answer) => true,
            { Required: true } when string.IsNullOrEmpty(answer) => false,
            not null when bool.TryParse(answer, out var value) => true,
            _ => false
        };
    }

    private bool IsValidOpenQuestion(OpenQuestion question, string? answer)
    {
        return question switch
        {
            { Required: true } when string.IsNullOrEmpty(answer) => false,
            _ => true
        };
    }
}