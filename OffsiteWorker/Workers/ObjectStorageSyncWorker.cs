using Core;
using Core.Services.Interfaces;

namespace OffsiteWorker.Workers;

public class ObjectStorageSyncWorker : BackgroundService
{
    private readonly ILogger<ObjectStorageSyncWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _syncInterval;

    public ObjectStorageSyncWorker(IConfiguration configuration, ILogger<ObjectStorageSyncWorker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _syncInterval = configuration.GetObjectStorageSyncInterval();
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var backupService = scope.ServiceProvider.GetService<IObjectStorageBackupService>()!;
        var cleanupService = scope.ServiceProvider.GetService<ICleanupService>()!;
        
        while (!cancellation.IsCancellationRequested)
        {
            _logger.LogInformation("Starting file sync...");
            int count = await backupService.BackupAsync();
            _logger.LogInformation("File sync finished, {count} files processed.", count);
            await cleanupService.CleanupObjectStorageBackupAsync();
            await Task.Delay(_syncInterval, cancellation);
        }
    }
}
