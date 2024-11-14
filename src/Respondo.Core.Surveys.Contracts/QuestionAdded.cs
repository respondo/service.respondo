namespace Respondo.Core.Surveys.Contracts;

public sealed record QuestionAdded
{
    /// <summary>
    ///     Id of the question that was added.
    /// </summary>
    public required Guid Id { get; init; }
}