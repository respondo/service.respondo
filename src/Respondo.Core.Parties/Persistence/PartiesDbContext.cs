using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Entities;

namespace Respondo.Core.Parties.Persistence;

public class PartiesDbContext(DbContextOptions<PartiesDbContext> options)
    : DbContext(options)
{
    public required DbSet<Member> Members { get; init; }
    public required DbSet<Occasion> Occasions { get; init; }
    public required DbSet<Party> Parties { get; init; }
    public required DbSet<Profile> Profiles { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(PartiesDbContext).Assembly);
    }
}