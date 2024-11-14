using Microsoft.EntityFrameworkCore;
using Respondo.Core.Surveys.Entities;

namespace Respondo.Core.Surveys.Persistence;

public class SurveysDbContext(DbContextOptions<SurveysDbContext> options)
    : DbContext(options)
{
    public required DbSet<Answer> Answers { get; init; }
    public required DbSet<Question> Questions { get; init; }
    public required DbSet<Survey> Surveys { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(SurveysDbContext).Assembly);
    }
}