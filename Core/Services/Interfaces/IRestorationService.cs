namespace Core.Services.Interfaces;

public interface IRestorationService
{
    public Task<int> RestoreObjectStorageAsync(string storageName);
}
