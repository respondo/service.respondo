using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Parties.Entities;

namespace Respondo.Core.Parties.Persistence.Configuration;

internal class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Occasions)
            .WithOne(e => e.Profile);
    }
}