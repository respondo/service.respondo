using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.PostgreSql;

namespace Respondo.Testing.Unit.Helpers;

public class DbContextFixture<T> : IAsyncLifetime where T : DbContext
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().Build();
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public T DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();

        var options = new DbContextOptionsBuilder<T>()
            .UseNpgsql(connectionString, builder => { builder.MigrationsAssembly("Respondo.Persistence.Migrations"); })
            .Options;

        DbContext = (T)Activator.CreateInstance(typeof(T), options)!;

        await DbContext.Database.MigrateAsync();

        _dbConnection = DbContext.Database.GetDbConnection();
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection,
            new RespawnerOptions
            {
                SchemasToInclude = new[] { "public" },
                DbAdapter = DbAdapter.Postgres
            }
        );
    }
    
    public Task ResetDatabase()
    {
        return _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task DisposeAsync()
    {
        await _dbConnection.CloseAsync();
        await _container.DisposeAsync();
    }
}