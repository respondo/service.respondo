namespace Respondo.Core.Surveys.Contracts;

public sealed record AnswersUpdated
{
    /// <summary>
    ///     Id of the survey related to the answer.
    /// </summary>
    public required Guid SurveyId { get; init; }
    
    /// <summary>
    ///     Id of the party that answered the survey.
    /// </summary>
    public required Guid PartyId { get; init; }
}