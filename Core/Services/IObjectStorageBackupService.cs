using Core.Models;

namespace Core.Services;

public interface IObjectStorageBackupService
{
    public Task<int> BackupAsync();
    
    /// <summary>
    /// Retrieves the file and archives it.
    /// </summary>
    /// <param name="file">info of the file to backup</param>
    /// <returns>updated info of the file</returns>
    public Task<ObjectStorageFile> BackupFileAsync(ObjectStorageFile file);

    /// <summary>
    /// Fetch the file from source and compress it to a location.
    /// </summary>
    /// <param name="source">the url of file</param>
    /// <param name="directoryPath">to put versioned files</param>
    /// <param name="filename">the name of file</param>
    /// <returns>the path to the file</returns>
    public Task<string> SaveFileAsync(string source, string directoryPath, string filename);
}
