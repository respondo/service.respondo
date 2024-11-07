using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Persistence.Configuration;

public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Statement)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(q => q.Required)
            .IsRequired();

        builder.HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Survey)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OpenQuestionEntityTypeConfiguration : IEntityTypeConfiguration<OpenQuestion>
{
    public void Configure(EntityTypeBuilder<OpenQuestion> builder)
    {
        builder.HasBaseType<Question>();
    }
}

public class GeneralQuestionEntityTypeConfiguration : IEntityTypeConfiguration<GeneralQuestion>
{
    public void Configure(EntityTypeBuilder<GeneralQuestion> builder)
    {
        builder.HasBaseType<Question>();
    }
}

public class SingleChoiceQuestionEntityTypeConfiguration : IEntityTypeConfiguration<SingleChoiceQuestion>
{
    public void Configure(EntityTypeBuilder<SingleChoiceQuestion> builder)
    {
        builder.HasBaseType<Question>();

        builder.Property(e => e.Options)
            .HasColumnName("Options");

        builder.Property(e => e.Options);
    }
}

public class MultipleChoiceQuestionEntityTypeConfiguration : IEntityTypeConfiguration<MultipleChoiceQuestion>
{
    public void Configure(EntityTypeBuilder<MultipleChoiceQuestion> builder)
    {
        builder.HasBaseType<Question>();

        builder.Property(e => e.Options)
            .HasColumnName("Options");

        builder.Property(e => e.Options);
    }
}