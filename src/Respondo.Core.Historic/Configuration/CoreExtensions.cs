using Marten.Events.Projections;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Respondo.Core.Historic.Aggregates;
using Respondo.Core.Historic.Consumers;
using Wolverine;

namespace Respondo.Core.Historic.Configuration;

/// <summary>
///     Extensions for configuring the Historic Core Module
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    ///     Configure the Historic Core Module.     
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
    public static void ConfigureHistoricModule(this WebApplicationBuilder builder)
    {
    }

    public static void AddHistoricModuleProjections(this ProjectionOptions options)
    {
        options.Add<OccasionProjection>(ProjectionLifecycle.Async);
    }
    
    /// <summary>
    ///     Include Wolverine handlers for Identity Core operations.
    /// </summary>
    /// <param name="options"><see cref="WolverineOptions"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void IncludeHistoricModule(this WolverineOptions options, IConfiguration configuration)
    {        
        // Module Handlers
        options.Discovery.IncludeType<GetOccasionHistoryHandler>();
        
        // Cross-Module Consumers
        options.Discovery.IncludeType<OccasionsConsumer>();
        options.Discovery.IncludeType<PartiesConsumer>();
    }
}