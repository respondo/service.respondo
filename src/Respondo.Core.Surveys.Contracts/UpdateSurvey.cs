namespace Respondo.Core.Surveys.Contracts;

public sealed record UpdateSurvey
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required Guid ProfileId { get; init; }
}