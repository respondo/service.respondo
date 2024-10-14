using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record GetPartyHandler
{
    public async Task<GetPartyResponse?> Handle(GetParty request, PartiesDbContext context)
    {
        throw new NotImplementedException();
    }
}