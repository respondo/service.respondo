using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Parties.Entities;

namespace Respondo.Core.Parties.Persistence.Configuration;

internal class PartyEntityTypeConfiguration : IEntityTypeConfiguration<Party>
{
    public void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.HasOne(e => e.Occasion)
            .WithMany(e => e.Parties);

        builder.HasMany(e => e.Members)
            .WithOne(e => e.Party);
    }
}