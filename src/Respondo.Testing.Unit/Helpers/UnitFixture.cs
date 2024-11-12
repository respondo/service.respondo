using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace Respondo.Testing.Unit.Helpers;

public class UnitFixture<T> : IAsyncLifetime where T : DbContext
{
    private readonly SqliteConnection _connection = new SqliteConnection("Filename=:memory:");
    
    public readonly TestMessageContext MessageContext = new();
    public T DbContext { get; private set; } = null!;
    

    public async Task InitializeAsync()
    {
        _connection.Open();

        var options = new DbContextOptionsBuilder<T>()
            .UseSqlite(_connection)
            .Options;

        
        DbContext = (T)Activator.CreateInstance(typeof(T), options)!;

        await DbContext.Database.EnsureCreatedAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _connection.CloseAsync();
    }
}