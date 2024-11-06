namespace Respondo.Core.Parties.Entities;

public sealed class Member
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Party Party { get; set; }
}