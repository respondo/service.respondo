using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Surveys.Persistence;
using Wolverine;

namespace Respondo.Core.Surveys.Configuration;

/// <summary>
///     Extensions for configuring the Surveys Core Module
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    ///     Configure the Surveys Core Module.     
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
    public static void ConfigureSurveysModule(this WebApplicationBuilder builder)
    {
        var identityDbConnectionString = builder.Configuration.GetConnectionString("SurveysDb");

        builder.Services.AddDbContext<SurveysDbContext>(options =>
        {
            options.UseNpgsql(identityDbConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(SurveysDbContext).Assembly.FullName);
            });
        });
    }

    /// <summary>
    ///     Run migrations for <see cref="SurveysDbContext"/>.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/>.</param>
    public static void RunSurveysDbMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<SurveysDbContext>().Database.Migrate();
    }
    
    /// <summary>
    ///     Include Wolverine handlers for Surveys Core operations.
    /// </summary>
    /// <param name="options"><see cref="WolverineOptions"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void IncludeSurveysModule(this WolverineOptions options, IConfiguration configuration)
    {
        // Module Handlers
        options.Discovery.IncludeType<AddQuestionHandler>();
        options.Discovery.IncludeType<AnswerSurveyHandler>();
        options.Discovery.IncludeType<CreateSurveyHandler>();
        options.Discovery.IncludeType<DeleteQuestionHandler>();
        options.Discovery.IncludeType<DeleteSurveyHandler>();
        options.Discovery.IncludeType<GetSurveyHandler>();
        options.Discovery.IncludeType<UpdateAnswersHandler>();
        options.Discovery.IncludeType<UpdateSurveyHandler>();
    }
}