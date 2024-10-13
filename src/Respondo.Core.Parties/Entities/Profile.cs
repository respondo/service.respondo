namespace Respondo.Core.Parties.Entities;

internal sealed class Profile
{
    public Profile()
    {
        Occasions = [];
    }
    
    public required Guid Id { get; set; }
    public List<Occasion> Occasions { get; set; }
}