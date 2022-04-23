using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core;

#nullable disable
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<ObjectStorageFile> ObjectStorageFiles { get; set; }
}