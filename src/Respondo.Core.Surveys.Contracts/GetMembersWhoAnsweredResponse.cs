namespace Respondo.Core.Surveys.Contracts;

public record GetMembersWhoAnsweredResponse
{
    public required IEnumerable<Guid> Members { get; init; }
}