using Core.Enums;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class RestorationService : IRestorationService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RestorationService> _logger;
    private readonly IObjectStorageProvider _storageProvider;
    
    public RestorationService(AppDbContext dbContext, ILogger<RestorationService> logger, IObjectStorageProvider storageProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _storageProvider = storageProvider;
    }
    
    public async Task<int> RestoreObjectStorageAsync(string storageName)
    {
        _logger.LogInformation("Starting object storage restoration process...");
        IQueryable<ObjectStorageFile> files = _dbContext.ObjectStorageFiles
            .Where(m => m.Storage == storageName && m.Status == SyncStatusEnum.Completed);

        // Does this still work if there are million files, or does it need to fetch e.g. 1000 first?
        var count = 0;
        foreach (ObjectStorageFile file in files)
        {
            try
            {
                await _storageProvider.RestoreFileAsync(file);
                count++;
            }
            catch (DirectoryNotFoundException)
            {
                file.Status = SyncStatusEnum.Deleted;
            }
        }
        _logger.LogInformation("Object storage restoration finished: {count} files uploaded.", count);

        return count;
    }
}
