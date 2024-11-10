using Marten.Events;
using Marten.Events.Aggregation;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Core.Historic.Aggregates;

public class OccasionProjection : SingleStreamProjection<Occasion>
{
    public Occasion Create(IEvent<OccasionCreated> @event)
    {
        return new Occasion
        {
            Id = @event.Data.OccasionId,
            Name = @event.Data.Name,
            CreatedAt = @event.Timestamp,
            UpdatedAt = @event.Timestamp
        };
    }

    #region Parties Core

    public Occasion Apply(IEvent<PartyCreated> @event, Occasion state)
    {
        state.Parties.Add(new Occasion.Party
        {
            Id = @event.Data.PartyId,
            Name = @event.Data.Name,
            CreatedAt = @event.Timestamp,
            UpdatedAt = @event.Timestamp
        });
        
        return state with
        {
            UpdatedAt = @event.Timestamp
        };
    }
    
    public Occasion Apply(IEvent<PartyUpdated> @event, Occasion state)
    {
        var party = state.Parties
            .Where(party => party.Id == @event.Data.PartyId)
            .Select(((party, i) => new { Value = party, Index = i }))
            .FirstOrDefault();

        if (party is null || party.Index < 0)
        {
            return state;
        }

        state.Parties[party.Index] = party.Value with
        {
            Name = @event.Data.Name,
            UpdatedAt = @event.Timestamp
        };

        return state with
        {
            UpdatedAt = @event.Timestamp
        };
    }

    #endregion
}