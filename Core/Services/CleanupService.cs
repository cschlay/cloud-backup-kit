using Core.Enums;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class CleanupService : ICleanupService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CleanupService> _logger;
    private readonly int _delayInSeconds;
    
    public CleanupService(AppDbContext dbContext, IConfiguration configuration, ILogger<CleanupService> logger)
    {
        _dbContext = dbContext;
        _delayInSeconds = configuration.GetCleanupDelay();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task CleanupAsync()
    {
        await CleanupObjectStorageBackupAsync();
    }

    /// <inheritdoc />
    public async Task CleanupObjectStorageBackupAsync()
    {
        var deleteCount = 0;
        _logger.LogInformation("Deleting backups of deleted files...");
        IEnumerable<ObjectStorageDeleteLog> deleteLogs = FindObjectDeleteLogs();
        foreach (ObjectStorageDeleteLog deleteLog in deleteLogs)
        {
            File.Delete(deleteLog.File!.BackupLocation);
            deleteLog.File.Status = SyncStatusEnum.Deleted;
            deleteLog.ConfirmedAt = DateTime.UtcNow;
            deleteCount += 1;
        }
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Backup file deletion completed: {count} files deleted", deleteCount);
    }

    private IEnumerable<ObjectStorageDeleteLog> FindObjectDeleteLogs()
    {
        DateTime deleteThreshold = DateTime.UtcNow.AddSeconds(-_delayInSeconds);
        return _dbContext.ObjectStorageDeleteLogs
            .Include(m => m.File)
            .Where(m => m.DeletedAt < deleteThreshold)
            .ToList();
    }
}
