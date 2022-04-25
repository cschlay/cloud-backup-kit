using Core.Models;
using Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class CleanupService : ICleanupService
{
    private readonly AppDbContext _dbContext;
    private readonly int _delayInSeconds;
    
    public CleanupService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _delayInSeconds = configuration.GetCleanupDelay();
    }

    /// <inheritdoc />
    public async Task CleanupAsync()
    {
        await CleanupObjectStorageBackupAsync();
    }

    /// <inheritdoc />
    public async Task CleanupObjectStorageBackupAsync()
    {
        IEnumerable<ObjectStorageDeleteLog> deleteLogs = FindObjectDeleteLogs();
        
    }

    private IEnumerable<ObjectStorageDeleteLog> FindObjectDeleteLogs()
    {
        DateTime deleteThreshold = DateTime.UtcNow.AddSeconds(-_delayInSeconds);
        return _dbContext.ObjectStorageDeleteLogs
            .Where(m => m.DeletedAt < deleteThreshold)
            .ToList();
    }
}
