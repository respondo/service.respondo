namespace Respondo.Core.Surveys.Contracts;

public sealed record DeleteSurvey
{
    public required Guid Id { get; init; }
    public required Guid ProfileId { get; init; }
}