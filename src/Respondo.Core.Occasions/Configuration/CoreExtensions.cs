using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Occasions.Persistence;
using Wolverine;

namespace Respondo.Core.Occasions.Configuration;

/// <summary>
///     Extensions for configuring the Occasions Core Module
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    ///     Configure the Occasions Core Module.     
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
    public static void ConfigureOccasionsModule(this WebApplicationBuilder builder)
    {
        var identityDbConnectionString = builder.Configuration.GetConnectionString("OccasionsDb");

        builder.Services.AddDbContext<OccasionDbContext>(options =>
        {
            options.UseNpgsql(identityDbConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(OccasionDbContext).Assembly.FullName);
                optionsBuilder.UseNodaTime();
            });
        });
    }

    /// <summary>
    ///     Run migrations for <see cref="OccasionDbContext"/>.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/>.</param>
    public static void RunOccasionsDbMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<OccasionDbContext>().Database.Migrate();
    }
    
    /// <summary>
    ///     Include Wolverine handlers for Identity Core operations.
    /// </summary>
    /// <param name="options"><see cref="WolverineOptions"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void IncludeOccasionsModule(this WolverineOptions options, IConfiguration configuration)
    {
        options.Discovery.IncludeType<CreateOccasionHandler>();
    }
}