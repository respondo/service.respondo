using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties.Consumers;

public sealed record OccasionCreatedConsumer
{
    public async Task Handle(OccasionCreated @event, PartiesDbContext context)
    {
        var profile = await context.Profiles.FirstAsync(profile => profile.Id == @event.ProfileId);
        
        var occasion = new Occasion
        {
            Id = @event.OccasionId,
            Profile = profile
        };
        
        await context.AddAsync(occasion);
        await context.SaveChangesAsync();
    }
}