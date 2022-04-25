using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Core.Utils;
using Microsoft.Extensions.Configuration;

namespace Core.Services;

public class ObjectStorageBackupService : IObjectStorageBackupService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileSanitizer _fileSanitizer;
    private readonly IHttpService _httpService;
    private readonly string _fileSystemRoot;
    
    public ObjectStorageBackupService(
        AppDbContext dbContext,
        IConfiguration configuration,
        IFileSanitizer fileSanitizer,
        IHttpService httpService)
    {
        _dbContext = dbContext;
        _fileSanitizer = fileSanitizer;
        _fileSystemRoot = configuration.GetFileSystemRoot();
        _httpService = httpService;
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
        if (file.Status == SyncStatusEnum.Completed) { return file; }

        try
        {
            string sanitizedPath = await _fileSanitizer.SanitizePathAsync(file.Path);
            string directoryPath = $"{_fileSystemRoot}/{file.Storage}/{sanitizedPath}";
            string filename = $"v{file.Version}.gzip";

            file.BackupLocation = await SaveFileAsync(file.SignedDownloadUrl, directoryPath, filename);
            file.SyncedAt = DateTime.UtcNow;
            file.Status = SyncStatusEnum.Completed;
        }
        catch (HttpResourceDoesNotExist)
        {
            file.Status = SyncStatusEnum.Deleted;
        }
        catch (ArgumentException)
        {
            // The file is unsafe, it is probably because the developers did it insecurely.
            file.Status = SyncStatusEnum.Declined;
        }
        catch (Exception)
        {
            // Failed could be retried later.
            file.Status = SyncStatusEnum.Failed;
        }

        return file;
    }

    /// <inheritdoc />
    public async Task<string> SaveFileAsync(string source, string directory, string filename)
    {
        Directory.CreateDirectory(directory);
        var path = $"{directory}/{filename}";
        Stream sourceStream = await _httpService.OpenHttpStreamAsync(source);
        await using FileStream targetStream = File.Create(path);
        FileTools.CompressAsync(sourceStream, targetStream);
        
        return path;
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
