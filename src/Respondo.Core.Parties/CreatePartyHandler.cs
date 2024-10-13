using Respondo.Core.Parties.Contracts;
using Wolverine;

namespace Respondo.Core.Parties;

public sealed record CreatePartyHandler
{
    public Task<(CreatePartyResponse, PartyCreated)> Handle(CreateParty request, Envelope envelope)
    {
        throw new NotImplementedException();
    }
}