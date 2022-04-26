using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Core.Enums;
using Core.Models;
using Core.Services;
using Core.Utils;
using Xunit;

namespace Tests.Core.Services;

public class RestorationServiceTest : DatabaseTestBase
{
    [Fact]
    public async Task RestoreObjectStorageAsyncTest()
    {
        Setup();
        var container = new BlobContainerClient("UseDevelopmentStorage=true", "test-restore");
        await container.DeleteIfExistsAsync();
        await container.CreateAsync();

        var storageProvider = new AzureBlobStorageProvider(configuration: Configuration);
        var service = new RestorationService(
            dbContext: DbContext,
            logger: MockLogger<RestorationService>(),
            storageProvider: storageProvider);
        await service.RestoreObjectStorageAsync("test-restore");

        IQueryable<ObjectStorageFile> files = DbContext.ObjectStorageFiles.Where(m => m.Storage == "test-restore");
        foreach (ObjectStorageFile file in files)
        {
            bool exists = await container.GetBlobClient(file.Path).ExistsAsync();
            Assert.True(exists);
        }
    }

    private void Setup()
    {
        const string directory = $"{TestConstants.FileDirectoryRoot}/test-restore";
        Directory.Delete(directory, true);
        Directory.CreateDirectory(directory);
        for (var i = 1; i <= 5; i++)
        {
            string path = $"{TestConstants.FileDirectoryRoot}/test-restore/file_{i}";
            FileStream fileStream = File.Create(path);
            fileStream.Write(Encoding.UTF8.GetBytes("Test Content"));
            fileStream.Close();
            
            DbContext.ObjectStorageFiles.Add(new ObjectStorageFile
            {
                Name = $"test-{i}",
                ContentType = "text/plain",
                Path = $"restored/test-{i}.txt",
                BackupLocation = path,
                Status = SyncStatusEnum.Completed,
                Storage = "test-restore",
                SyncedAt = DateTime.UtcNow,
            });
        }
        DbContext.SaveChanges();
    }
}
