using System.Text.Json.Serialization;
using Respondo.Core.Identity.Configuration;
using Respondo.Core.Occasions.Configuration;
using Respondo.Core.Parties.Configuration;
using Respondo.Core.Surveys.Configuration;
using Scalar.AspNetCore;
using Wolverine;

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
        builder.ConfigureSurveysModule();
        
        // builder.UseWolverine(options =>
        // {
        //     options.Discovery.DisableConventionalDiscovery();
        //
        //     options.IncludeIdentityCore(builder.Configuration);
        //     options.IncludeOccasionsModule(builder.Configuration);
        //     options.IncludePartiesModule(builder.Configuration);
        //     options.IncludeSurveysModule(builder.Configuration);
        // });
        
        var app = builder.Build();
        
        app.UseCors(options =>
        {
            options.WithOrigins("https://08fbaaf0.respondo-dashboard.pages.dev")
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
            app.RunSurveysDbMigrations();
        }
        
        app.Run();
    }
}