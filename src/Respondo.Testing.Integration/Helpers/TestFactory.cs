using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Identity.Persistence;
using Respondo.Core.Occasions.Persistence;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Testing.Integration.Helpers;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddJsonFile("appsettings.Local.json", optional: true);
            configurationBuilder.AddJsonFile("appsettings.Testing.json", optional: true);
        });

        
        builder.ConfigureTestServices(services =>
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            services.RemoveDbContext<IdentityDbContext>();
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(GenerateConnectionString("identity"), optionsBuilder => { });
            });
            
            services.EnsureDbCreated<IdentityDbContext>();
            
            services.RemoveDbContext<OccasionDbContext>();
            services.AddDbContext<OccasionDbContext>(options =>
            {
                options.UseNpgsql(GenerateConnectionString("occasions"), optionsBuilder => { });
            });
            
            services.EnsureDbCreated<OccasionDbContext>();
            
            services.RemoveDbContext<PartiesDbContext>();
            services.AddDbContext<PartiesDbContext>(options =>
            {
                options.UseNpgsql(GenerateConnectionString("parties"), optionsBuilder => { });
            });
            
            services.EnsureDbCreated<PartiesDbContext>();
        });
    }

    public HttpClient CreateClientWithoutRedirect()
    {
        var options = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        };
        
        return CreateClient(options);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private string GenerateConnectionString(string database)
    {
        return $"Server=127.0.0.1;Port=5433;Database=respondo.{database};User Id=testing;Password=testing;";
        // return _container.GetConnectionString().Replace("respondo", $"respondo.{database}");
    }
}