using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Contracts.Entities;

namespace Respondo.Core.Occasions.Persistence;

public class OccasionDbContext(DbContextOptions<OccasionDbContext> options)
    : DbContext(options)
{
    public required DbSet<Occasion> Occasions { get; init; }
    public required DbSet<Profile> Profiles { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(OccasionDbContext).Assembly);
    }
}