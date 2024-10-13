namespace Respondo.Core.Parties.Entities;

internal sealed class Member
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
}