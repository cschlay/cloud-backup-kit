using Core.Models;

namespace Core.Services.Interfaces;

public interface IObjectStorageProvider
{
    public Task RestoreFileAsync(ObjectStorageFile file);
}
