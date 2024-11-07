using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Persistence.Configuration;

public class SurveyEntityTypeConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.HasMany(e => e.Questions)
            .WithOne(e => e.Survey)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.OccasionId)
            .IsRequired();
    }
}