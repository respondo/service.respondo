namespace Respondo.Core.Parties.Entities;

public sealed class Party
{
    public Party()
    {
        Members = [];
    }

    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public required Occasion Occasion { get; set; }
    public List<Member> Members { get; set; }
}