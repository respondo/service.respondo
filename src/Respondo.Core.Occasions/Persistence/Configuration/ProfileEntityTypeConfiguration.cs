using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Occasions.Contracts.Entities;

namespace Respondo.Core.Occasions.Persistence.Configuration;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Occasions)
            .WithOne(e => e.Profile);
    }
}