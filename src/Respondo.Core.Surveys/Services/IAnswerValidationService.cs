using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Services;

/// <summary>
///     A service that validates answers against questions.
/// </summary>
public interface IAnswerValidationService
{
    /// <summary>
    ///     Determines if the given <paramref name="answer" /> is valid according to the given <paramref name="question" />.
    /// </summary>
    /// <param name="question">The question to validate against.</param>
    /// <param name="answer">The answer to validate.</param>
    /// <returns>
    ///     <see langword="true" /> if the given <paramref name="answer" /> is valid according to the given
    ///     <paramref name="question" />, <see langword="false" /> otherwise.
    /// </returns>
    public bool IsValid(Question question, string? answer);
}