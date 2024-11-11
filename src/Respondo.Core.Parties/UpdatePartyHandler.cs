using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record UpdatePartyHandler
{
    public async Task<PartyUpdated?> Handle(UpdateParty request, PartiesDbContext context, ILogger<UpdatePartyHandler> logger)
    {
        var party = await context.Parties
            .Where(party => party.Occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(party => party.Id == request.PartyId);

        if (party is null)
        {
            logger.LogError("Party with id {PartyId} not found but update was requested", request.PartyId);
            
            return default;
        }

        if (request.Name is { Length: > 0 } name)
        {
            party.Name = name;
        }
        
        if (request.Email is { Length: > 0 } email)
        {
            party.Email = email;
        }

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
            
            return new PartyUpdated { PartyId = party.Id, Name = party.Name, OccasionId = request.OccasionId };
        }

        return default;
    }
}