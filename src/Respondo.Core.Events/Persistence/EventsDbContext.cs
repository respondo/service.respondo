using Microsoft.EntityFrameworkCore;
using Respondo.Core.Events.Contracts.Entities;

namespace Respondo.Core.Events.Persistence;

public class EventsDbContext(DbContextOptions<EventsDbContext> options)
    : DbContext(options)
{
    public required DbSet<Event> Events { get; init; }
    public required DbSet<Profile> Profiles { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);
    }
}