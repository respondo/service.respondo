namespace Respondo.Core.Events.Contracts.Entities;

public class Profile
{
    public Profile()
    {
        Events = [];
    }
    
    public required Guid Id { get; set; }
    
    public List<Event> Events { get; set; }
}