using Microsoft.EntityFrameworkCore;

namespace Core;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}