namespace Respondo.Core.Parties.Entities;

internal sealed class Occasion
{
    public Occasion()
    {
        Parties = [];
    }
    
    public required Guid Id { get; set; }
    public required Profile Profile { get; set; }
    public List<Party> Parties { get; set; }
}