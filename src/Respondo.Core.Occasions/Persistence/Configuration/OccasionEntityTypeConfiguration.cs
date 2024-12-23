using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Occasions.Entities;

namespace Respondo.Core.Occasions.Persistence.Configuration;

public class OccasionEntityTypeConfiguration : IEntityTypeConfiguration<Occasion>
{
    public void Configure(EntityTypeBuilder<Occasion> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.BaseUrl)
            .HasMaxLength(100);

        builder.Property(e => e.BaseUrl);

        builder.HasOne(e => e.Profile)
            .WithMany(e => e.Occasions);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.LastUpdatedAt)
            .IsRequired();
    }
}