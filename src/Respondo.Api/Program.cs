using System.Text.Json.Serialization;
using JasperFx.Core;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Respondo.Core.Historic.Configuration;
using Respondo.Core.Identity.Configuration;
using Respondo.Core.Occasions.Configuration;
using Respondo.Core.Parties.Configuration;
using Scalar.AspNetCore;
using Weasel.Core;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Marten;

namespace Respondo.Api;

/// <summary>
///     Respondo.
/// </summary>
public class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

        builder.Services.AddControllers()
            .AddJsonOptions(options => { options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

        builder.Services.AddOpenApi();
        
        builder.Services.AddSingleton(TimeProvider.System);

        builder.ConfigureIdentityModule();
        builder.ConfigureOccasionsModule();
        builder.ConfigurePartiesModule();
        
        builder.Services.AddMarten(options =>
        {
            options.Connection(builder.Configuration.GetConnectionString("HistoricDb")!);
            options.UseSystemTextJsonForSerialization();
            options.Projections.AddHistoricModuleProjections();
            options.AutoCreateSchemaObjects = builder.Environment.IsProduction() 
                ? AutoCreate.CreateOrUpdate 
                : AutoCreate.All;
        }).ApplyAllDatabaseChangesOnStartup().AddAsyncDaemon(DaemonMode.HotCold).IntegrateWithWolverine();
        
        builder.UseWolverine(options =>
        {
            options.Discovery.DisableConventionalDiscovery();

            options.Policies.OnAnyException()
                .RetryWithCooldown(100.Milliseconds(), 250.Milliseconds(), 500.Milliseconds());
            
            options.Policies.AutoApplyTransactions();
            
            options.Policies.UseDurableInboxOnAllListeners();
            options.Policies.UseDurableOutboxOnAllSendingEndpoints();
            
            options.IncludeHistoricModule(builder.Configuration);
            options.IncludeIdentityCore(builder.Configuration);
            options.IncludeOccasionsModule(builder.Configuration);
            options.IncludePartiesModule(builder.Configuration);
        });
        
        var app = builder.Build();
        
        app.UseCors(options =>
        {
            options.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {   
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapGet("/", () => Results.LocalRedirect("/scalar/v1", permanent: true));
        }
        
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.RunIdentityDbMigrations();
            app.RunOccasionsDbMigrations();
            app.RunPartiesDbMigrations();
        }
        
        app.Run();
    }
}