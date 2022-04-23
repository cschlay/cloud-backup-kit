namespace OffsiteWorker.Workers;

public class ObjectStorageSyncWorker : BackgroundService
{
    private readonly ILogger<ObjectStorageSyncWorker> _logger;

    public ObjectStorageSyncWorker(ILogger<ObjectStorageSyncWorker> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            _logger.LogInformation("Starting file sync...");
            _logger.LogInformation("File sync finished, {count} files downloaded.", 2);
            await Task.Delay(5000, cancellation);
        }
    }
}
