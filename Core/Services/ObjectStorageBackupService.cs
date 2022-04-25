using System.IO.Compression;
using Core.Enums;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class ObjectStorageBackupService : IObjectStorageBackupService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileSanitizer _sanitizer;
    private readonly IHttpService _httpService;
    
    public ObjectStorageBackupService(
        AppDbContext dbContext,
        IFileSanitizer sanitizer,
        IHttpService httpService)
    {
        _dbContext = dbContext;
        _httpService = httpService;
        _sanitizer = sanitizer;
    }

    public async Task BackupAsync()
    {
        IEnumerable<ObjectStorageFile> pending = FindPendingFiles();
        // TODO: Queue the items
        // TODO: Download in batches
        
        await _dbContext.SaveChangesAsync();

    }

    /// <inheritdoc />
    public async Task<ObjectStorageFile> BackupFileAsync(ObjectStorageFile file)
    {
        if (file.SyncedAt != null) { return file; }

        string sanitizedPath = await _sanitizer.SanitizePathAsync(file.Path);
        string sanitizedName = await _sanitizer.SanitizeFileNameAsync(file.Name);

        file.BackupLocation = $"{file.Storage}/{sanitizedPath}/{sanitizedName}/v{file.Version}.gzip";
        file.SyncedAt = DateTime.UtcNow;

        await SaveFileAsync(file.SignedDownloadUrl, file.BackupLocation);
        
        return file;
    }

    /// <inheritdoc />
    public async Task SaveFileAsync(string source, string target)
    {
        Stream sourceStream = await _httpService.OpenHttpStreamAsync(source);
        await using FileStream targetStream = File.Create(target);
        FileTools.CompressAsync(sourceStream, targetStream);
    }
    
    /// <summary>
    /// Finds the pending files assigned to to a the worker. The files that are not synced.
    /// </summary>
    /// <returns>the pending files</returns>
    private IEnumerable<ObjectStorageFile> FindPendingFiles()
    {
        return _dbContext.ObjectStorageFiles
            .Where(m => m.SyncedAt == null && m.Status == SyncStatusEnum.Pending)
            .ToList();
    }
}
