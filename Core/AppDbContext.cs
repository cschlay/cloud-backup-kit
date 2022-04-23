using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core;

#nullable disable
public class AppDbContext : DbContext
{
    /// <summary>For test mocks only, do not use!</summary>
    public AppDbContext() { }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<ObjectStorageFile> ObjectStorageFiles { get; set; }
}