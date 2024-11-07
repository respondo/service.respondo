namespace Respondo.Core.Surveys.Contracts;

public sealed record CreateSurvey
{
    public required Guid OccasionId { get; init; }
    public required string Title { get; init; }
    public required Guid ProfileId { get; init; }
}