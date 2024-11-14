namespace Respondo.Core.Surveys.Contracts;

public sealed record SurveyAnswered
{
    /// <summary>
    ///     Id of the survey that was answered.
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    ///     Id of the party that answered the survey.
    /// </summary>
    public required Guid PartyId { get; init; }
}