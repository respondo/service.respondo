namespace Respondo.Core.Surveys.Contracts;

public sealed record GetSurveyByParty
{
    public required Guid PartyId { get; init; }
}