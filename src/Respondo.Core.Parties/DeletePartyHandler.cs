using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record DeletePartyHandler
{
    public async Task<PartyDeleted?> Handle(DeleteParty request, PartiesDbContext context)
    {
        var party = await context.Parties
            .Where(party => party.Occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(party => party.Id == request.Id);

        if (party is null)
        {
            return default;
        }

        context.Parties.Remove(party);
        await context.SaveChangesAsync();

        return new PartyDeleted { Id = party.Id };
    }
}