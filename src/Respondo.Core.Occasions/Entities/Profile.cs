namespace Respondo.Core.Occasions.Entities;

public class Profile
{
    public Profile()
    {
        Occasions = [];
    }
    
    public required Guid Id { get; set; }
    
    public List<Occasion> Occasions { get; set; }
}