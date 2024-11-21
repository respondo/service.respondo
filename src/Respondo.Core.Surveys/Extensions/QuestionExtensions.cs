using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Extensions;

public static class QuestionExtensions
{
    /// <summary>
    ///     Retrieves the options from a <see cref="Question"/> or null if the question is not a choice question.
    /// </summary>
    /// <param name="question">The question to retrieve options from.</param>
    /// <returns>The options from the question, or null if the question is not a choice question.</returns>
    public static List<string>? RetrieveOptions(this Question question)
    {
        return question switch
        {
            SingleChoiceQuestion singleChoiceQuestion => singleChoiceQuestion.Options,
            MultipleChoiceQuestion multipleChoiceQuestion => multipleChoiceQuestion.Options,
            _ => default
        };
    }
}