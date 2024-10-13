using Respondo.Core.Identity.Contracts;
using Respondo.Core.Occasions.Entities;
using Respondo.Core.Occasions.Persistence;

namespace Respondo.Core.Occasions.Consumers;

public record CreateApplicationUserConsumer
{
    public async Task Handle(ApplicationUserCreated @event, OccasionDbContext context)
    {
        var profile = new Profile
        {
            Id = @event.ApplicationUserId
        };

        await context.AddAsync(profile);

        await context.SaveChangesAsync();
    }
}