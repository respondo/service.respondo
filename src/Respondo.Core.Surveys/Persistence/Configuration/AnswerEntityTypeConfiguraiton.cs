using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Persistence.Configuration;

public class AnswerEntityTypeConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Value)
            .HasMaxLength(4096);

        builder.HasOne(e => e.Question)
            .WithMany(e => e.Answers)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.PartyId)
            .IsRequired();
    }
}