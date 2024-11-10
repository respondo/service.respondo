using Marten;
using Respondo.Core.Occasions.Contracts;

namespace Respondo.Core.Historic.Consumers;

public sealed record OccasionsConsumers
{
    public void Consumer(OccasionCreated @event, IDocumentSession session)
    {
        session.Events.StartStream(@event.OccasionId, @event);
    }
}