using Respondo.Core.Identity.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties.Consumers;

public record ApplicationUserCreatedConsumer
{
    public async Task Handle(ApplicationUserCreated @event, PartiesDbContext context)
    {
        var profile = new Profile
        {
            Id = @event.ApplicationUserId
        };

        await context.AddAsync(profile);
        await context.SaveChangesAsync();
    }
}