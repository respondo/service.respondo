using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respondo.Persistence.Context;
using Testcontainers.PostgreSql;

namespace Respondo.Testing.Integration.Helpers;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .WithPortBinding(45432, 5432)
        .WithReuse(true)
        .WithLabel("reuse-id", "respondo.testing.integration")
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

    private static string GenerateConnectionString(string database)
    {
        return $"Host=127.0.0.1;Port=45432;Database=respondo.{database};Username=postgres;Password=postgres";
    }
}