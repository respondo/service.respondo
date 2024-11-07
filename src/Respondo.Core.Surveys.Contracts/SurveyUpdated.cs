namespace Respondo.Core.Surveys.Contracts;

public sealed record SurveyUpdated
{
    /// <summary>
    ///     Id of the survey that was updated.
    /// </summary>
    public required Guid Id { get; init; }
}