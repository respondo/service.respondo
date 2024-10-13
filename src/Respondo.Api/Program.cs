using System.Text.Json.Serialization;
using Respondo.Core.Identity.Configuration;
using Respondo.Core.Occasions.Configuration;
using Wolverine;

namespace Respondo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

        builder.Services.AddControllers()
            .AddJsonOptions(options => { options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

        builder.Services.AddSingleton(TimeProvider.System);

        builder.ConfigureIdentityModule();
        builder.ConfigureOccasionsModule();
        
        builder.UseWolverine(options =>
        {
            options.Discovery.DisableConventionalDiscovery();

            options.IncludeIdentityCore(builder.Configuration);
            options.IncludeOccasionsModule(builder.Configuration);
        });
        
        var app = builder.Build();

        app.MapControllers();

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.RunIdentityDbMigrations();
            app.RunOccasionsDbMigrations();
        }
        
        app.Run();
    }
}