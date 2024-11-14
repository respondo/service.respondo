namespace Respondo.Core.Surveys.Contracts;

public sealed record SurveyDeleted
{
    /// <summary>
    ///     Id of the survey that was deleted.
    /// </summary>
    public required Guid Id { get; init; }
}