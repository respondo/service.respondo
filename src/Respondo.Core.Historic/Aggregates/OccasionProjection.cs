using Marten.Events;
using Marten.Events.Aggregation;
using Respondo.Core.Occasions.Contracts;

namespace Respondo.Core.Historic.Aggregates;

public class OccasionProjection : SingleStreamProjection<Occasion>
{
    public Occasion Create(IEvent<OccasionCreated> @event)
    {
        return new Occasion
        {
            Id = @event.Data.OccasionId,
            CreatedAt = @event.Timestamp
        };
    }
}