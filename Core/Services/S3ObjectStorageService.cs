using Core.Models;

namespace Core.Services;

public class S3ObjectStorageService : IObjectStorageService
{
    public Task<string> DownloadFileAsync(ObjectStorageFile file)
    {
        throw new NotImplementedException();
    }
}