using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record DeletePartyMemberHandler
{
    public Task<PartyMemberDeleted?> Handle(DeletePartyMember request, PartiesDbContext context)
    {
        throw new NotImplementedException();
    }
}