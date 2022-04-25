using Core.Models;

namespace Core.Services;

public class ObjectStorageService : IObjectStorageService
{
    public ObjectStorageService()
    {
    }

    public Task<string> DownloadFileAsync(string url)
    {
        throw new NotImplementedException();
    }
}
