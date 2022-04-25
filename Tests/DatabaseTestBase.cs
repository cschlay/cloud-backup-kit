using Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

[Collection("WithDatabase")]
public class DatabaseTestBase : TestBase
{
    protected readonly AppDbContext DbContext;
    private const string ConnectionString = "Database=test;Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;";
    
    protected DatabaseTestBase()
    {
        var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
        dbContextBuilder.UseNpgsql(ConnectionString);

        DbContext = new AppDbContext(dbContextBuilder.Options);
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }
}
