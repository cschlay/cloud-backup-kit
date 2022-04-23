using System.IO.Compression;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class ObjectStorageBackupService : IObjectStorageBackupService
{
    private readonly AppDbContext _dbContext;
    private readonly IFileSanitizer _sanitizer;
    private readonly IObjectStorageService _storage;
    
    public ObjectStorageBackupService(AppDbContext dbContext, IFileSanitizer sanitizer, IObjectStorageService storage)
    {
        _dbContext = dbContext;
        _sanitizer = sanitizer;
        _storage = storage;
    }

    public async Task<ObjectStorageFile> BackupFileAsync(ObjectStorageFile file)
    {
        if (file.SyncedAt != null) { return file; }

        string sanitizedPath = await _sanitizer.SanitizePathAsync(file.Path);
        string sanitizedName = await _sanitizer.SanitizePathAsync(file.Name);

        file.BackupLocation = $"{file.Storage}/{sanitizedPath}/{sanitizedName}/v{file.Version}.gzip";
        file.SyncedAt = DateTime.UtcNow;

        string sourcePath = await _storage.DownloadFileAsync(file);
        await CompressAndSaveFileAsync(sourcePath, file.BackupLocation);
        await _dbContext.SaveChangesAsync();
        
        return file;
    }
    
    /// <summary>
    /// Finds the pending files assigned to to a the worker. The files that are not synced.
    /// </summary>
    /// <returns>the pending files</returns>
    public List<ObjectStorageFile> FindPendingFiles()
    {
        return _dbContext.ObjectStorageFiles.Where(m => m.SyncedAt == null).ToList();
    }

    // https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.gzipstream
    private static async Task CompressAndSaveFileAsync(string source, string target)
    {
        await using FileStream sourceStream = File.Open(source, FileMode.Open);
        await using FileStream targetStream = File.Create(target);
        await using var gzip = new GZipStream(targetStream, CompressionMode.Compress);
        await sourceStream.CopyToAsync(gzip);
    }
}
