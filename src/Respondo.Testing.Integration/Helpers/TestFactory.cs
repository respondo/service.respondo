using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Core.Occasions.Persistence;
using Respondo.Core.Parties.Persistence;
using Respondo.Persistence.Context;
using Testcontainers.PostgreSql;
using Wolverine.Configuration;
using Wolverine.Runtime;

namespace Respondo.Testing.Integration.Helpers;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithLogger(default)
        .WithDatabase("respondo")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

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
                options.UseNpgsql(GenerateConnectionString("identity"), optionsBuilder =>
                {
                    optionsBuilder
                        .MigrationsAssembly("Respondo.Persistence.Migrations");
                });
            });
            
            services.EnsureDbCreated<IdentityDbContext>();
            
            services.RemoveDbContext<OccasionDbContext>();
            services.AddDbContext<OccasionDbContext>(options =>
            {
                options.UseNpgsql(GenerateConnectionString("occasions"), optionsBuilder =>
                {
                    optionsBuilder.UseNodaTime();
                });
            });
            
            services.EnsureDbCreated<OccasionDbContext>();
            
            services.RemoveDbContext<PartiesDbContext>();
            services.AddDbContext<PartiesDbContext>(options =>
            {
                options.UseNpgsql(GenerateConnectionString("parties"), optionsBuilder =>
                {
                    optionsBuilder.UseNodaTime();
                });
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

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    private string GenerateConnectionString(string database)
    {
        return _container.GetConnectionString().Replace("respondo", $"respondo.{database}");
    }
}