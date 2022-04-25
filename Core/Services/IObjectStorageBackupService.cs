using Core.Models;

namespace Core.Services;

public interface IObjectStorageBackupService
{
    public Task BackupAsync();
    
    /// <summary>
    /// Retrieves the file and archives it.
    /// </summary>
    /// <param name="file">info of the file to backup</param>
    /// <returns>updated info of the file</returns>
    public Task<ObjectStorageFile> BackupFileAsync(ObjectStorageFile file);
    
    
}
