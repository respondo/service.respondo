namespace Respondo.Core.Parties.Entities;

public sealed class Profile
{
    public Profile()
    {
        Occasions = [];
    }
    
    public required Guid Id { get; set; }
    public List<Occasion> Occasions { get; set; }
}