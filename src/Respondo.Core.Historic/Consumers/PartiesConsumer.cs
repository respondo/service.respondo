using Marten;
using Respondo.Core.Parties.Contracts;

namespace Respondo.Core.Historic.Consumers;

public sealed record PartiesConsumer
{
    public async Task Consume(PartyCreated @event, IDocumentSession session)
    {
        await session.Events.AppendExclusive(@event.OccasionId, @event);
    }

    public async Task Consume(PartyUpdated @event, IDocumentSession session)
    {
        await session.Events.AppendExclusive(@event.OccasionId, @event);
    }
    
    public async Task Consume(PartyDeleted @event, IDocumentSession session)
    {
        await session.Events.AppendExclusive(@event.OccasionId, @event);
    }
}