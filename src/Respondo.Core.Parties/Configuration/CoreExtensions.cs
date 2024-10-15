using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Parties.Consumers;
using Respondo.Core.Parties.Persistence;
using Wolverine;

namespace Respondo.Core.Parties.Configuration;

/// <summary>
///     Extensions for configuring the Parties Core Module
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    ///     Configure the Parties Core Module.     
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
    public static void ConfigurePartiesModule(this WebApplicationBuilder builder)
    {
        var identityDbConnectionString = builder.Configuration.GetConnectionString("PartiesDb");

        builder.Services.AddDbContext<PartiesDbContext>(options =>
        {
            options.UseNpgsql(identityDbConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(PartiesDbContext).Assembly.FullName);
                optionsBuilder.UseNodaTime();
            });
        });
    }

    /// <summary>
    ///     Run migrations for <see cref="PartiesDbContext"/>.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/>.</param>
    public static void RunPartiesDbMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<PartiesDbContext>().Database.Migrate();
    }
    
    /// <summary>
    ///     Include Wolverine handlers for Parties Core operations.
    /// </summary>
    /// <param name="options"><see cref="WolverineOptions"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void IncludePartiesModule(this WolverineOptions options, IConfiguration configuration)
    {
        // Module Handlers
        options.Discovery.IncludeType<AddMemberToPartyHandler>();
        options.Discovery.IncludeType<CreatePartyHandler>();
        options.Discovery.IncludeType<GetPartyHandler>();
        
        // Cross-Module Consumers
        options.Discovery.IncludeType<ApplicationUserCreatedConsumer>();
        options.Discovery.IncludeType<OccasionCreatedConsumer>();
    }
}