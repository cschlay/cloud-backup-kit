using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core;

#nullable disable
public class AppDbContext : DbContext
{
    public DbSet<ObjectStorageDeleteLog> ObjectStorageDeleteLogs { get; set; }
    public DbSet<ObjectStorageFile> ObjectStorageFiles { get; set; }
    
    /// <summary>For test mocks only, do not use!</summary>
    public AppDbContext() { }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ObjectStorageFileConfiguration());
    }
}
