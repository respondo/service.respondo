using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record CreatePartyHandler
{
    public async Task<(CreatePartyResponse?, PartyCreated?)> Handle(CreateParty request, PartiesDbContext context)
    {
        var occasion = await context.Occasions
            .Where(occasion => occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(occasion => occasion.Id == request.OccasionId);

        if (occasion is null)
        {
            return default;
        }

        var party = new Party
        {
            Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow()),
            Name = request.Name,
            Email = request.Email,
            Occasion = occasion
        };
        
        await context.Parties.AddAsync(party);
        await context.SaveChangesAsync();
        
        return (new CreatePartyResponse { Id = party.Id }, new PartyCreated { Id = party.Id });
    }
}