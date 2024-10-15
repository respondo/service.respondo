using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record DeletePartyHandler
{
    public Task<PartyDeleted?> Handle(DeleteParty request, PartiesDbContext context)
    {
        throw new NotImplementedException();
    }
}