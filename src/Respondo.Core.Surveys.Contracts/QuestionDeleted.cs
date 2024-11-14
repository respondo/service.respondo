namespace Respondo.Core.Surveys.Contracts;

public sealed record QuestionDeleted
{
    /// <summary>
    ///     Id of the question that was deleted.
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    ///     Id of the survey related to the question.
    /// </summary>
    public required Guid SurveyId { get; init; }
}