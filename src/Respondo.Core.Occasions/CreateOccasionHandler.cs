using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Persistence;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Entities;

namespace Respondo.Core.Occasions;

public sealed record CreateOccasionHandler
{
    public async Task<(CreateOccasionResponse, OccasionCreated)> Handle(CreateOccasion request, OccasionDbContext context)
    {
        var profile = await context.Profiles.FirstAsync(profile => profile.Id == request.ProfileId);

        var occasion = new Occasion
        {
            Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow()),
            Name = request.Name,
            Profile = profile
        };

        await context.Occasions.AddAsync(occasion);

        await context.SaveChangesAsync();

        return (new CreateOccasionResponse { Id = occasion.Id }, new OccasionCreated { 
            OccasionId = occasion.Id, 
            Name = occasion.Name,
            ProfileId = profile.Id });
    }
}