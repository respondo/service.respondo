using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace Respondo.Testing.Unit.Helpers;

public class DbContextFixture<T> : IAsyncLifetime where T : DbContext
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithCleanUp(true)
        .Build();

    internal T DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();

        var options = new DbContextOptionsBuilder<T>()
            .UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(T).Assembly.GetName().Name);
                builder.UseNodaTime();
            })
            .Options;

        
        DbContext = (T)Activator.CreateInstance(typeof(T), options)!;

        await DbContext.Database.EnsureCreatedAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}