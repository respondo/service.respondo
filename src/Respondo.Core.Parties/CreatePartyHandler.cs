using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;
using Wolverine;

namespace Respondo.Core.Parties;

public sealed record CreatePartyHandler
{
    public Task<(CreatePartyResponse, PartyCreated)> Handle(CreateParty request, PartiesDbContext context)
    {
        throw new NotImplementedException();
    }
}