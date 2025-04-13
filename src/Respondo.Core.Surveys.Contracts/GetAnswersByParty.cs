namespace Respondo.Core.Surveys.Contracts;

public sealed record GetAnswersByParty
{
    public required Guid PartyId { get; init; }
}
