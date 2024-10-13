using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record AddMemberToPartyHandler
{
    public async Task Handle(AddMemberToParty request, PartiesDbContext context)
    {
        throw new NotImplementedException();
    }
}