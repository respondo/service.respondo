namespace Respondo.Core.Surveys.Contracts;

public sealed record SurveyCreated
{
    /// <summary>
    ///     Id of the survey that was created.
    /// </summary>
    public required Guid Id { get; init; }
}