using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Parties.Entities;

namespace Respondo.Core.Parties.Persistence.Configuration;

internal class OccasionEntityTypeConfiguration : IEntityTypeConfiguration<Occasion>
{
    public void Configure(EntityTypeBuilder<Occasion> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Parties)
            .WithOne(e => e.Occasion);
        
        builder.HasOne(e => e.Profile)
            .WithMany(e => e.Occasions);
    }
}