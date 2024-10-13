namespace Respondo.Core.Parties.Contracts;

public sealed record MemberAdded
{
    public required Guid Id { get; init; }
}