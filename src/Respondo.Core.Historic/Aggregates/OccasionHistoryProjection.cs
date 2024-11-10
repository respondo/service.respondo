using Marten.Events;
using Marten.Events.Aggregation;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Core.Historic.Aggregates;

public sealed class OccasionHistoryProjection : SingleStreamProjection<OccasionHistory>
{
    public OccasionHistory Create(IEvent<OccasionCreated> @event)
    {
        var state = new OccasionHistory
        {
            Id = @event.Data.OccasionId,
        };
        
        state.History.Add(new OccasionHistory.Event
        {
            Log = $"{nameof(OccasionCreated)}: {@event.Data.Name}",
            Timestamp = @event.Timestamp,
        });
        
        return state; 
    }
    
    public OccasionHistory Apply(IEvent<PartyCreated> @event, OccasionHistory state)
    {
        state.History.Add(new OccasionHistory.Event
        {
            Log = $"{nameof(PartyCreated)}: {@event.Data.Name} ({@event.Data.PartyId})",
            Timestamp = @event.Timestamp,
        });
        
        return state;
    }
    
    public OccasionHistory Apply(IEvent<PartyUpdated> @event, OccasionHistory state)
    {
        state.History.Add(new OccasionHistory.Event
        {
            Log = $"{nameof(PartyUpdated)}: {@event.Data.Name} ({@event.Data.PartyId})",
            Timestamp = @event.Timestamp,
        });
        
        return state;
    }
    
    public OccasionHistory Apply(IEvent<PartyDeleted> @event, OccasionHistory state)
    {
        state.History.Add(new OccasionHistory.Event
        {
            Log = $"{nameof(PartyDeleted)}: ({@event.Data.PartyId})",
            Timestamp = @event.Timestamp,
        });
        
        return state;
    }
}