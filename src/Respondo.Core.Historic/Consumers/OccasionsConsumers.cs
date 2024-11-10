using Marten;
using Respondo.Core.Occasions.Contracts;

namespace Respondo.Core.Historic.Consumers;

public sealed record OccasionsConsumer
{
    public void Consume(OccasionCreated @event, IDocumentSession session)
    {
        session.Events.StartStream(@event.OccasionId, @event);
    }
}