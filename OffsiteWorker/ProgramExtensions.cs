using Core;
using Microsoft.EntityFrameworkCore;

namespace OffsiteWorker;

public static class ProgramExtensions
{
    public static void AddDatabase(this IServiceCollection services, HostBuilderContext host)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            string connectionString = host.Configuration.GetConnectionString("PostgreSQL");
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("OffsiteWorker");
            });
        });
    }
}
