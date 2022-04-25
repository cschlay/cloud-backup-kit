using System;
using System.IO;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models;
using Core.Services;
using Xunit;

namespace Tests.Core.Services;

public class CleanupServiceTest : DatabaseTestBase
{
    private const string DirectoryPath = $"{TestConstants.FileDirectoryRoot}/CleanupTest";

    public CleanupServiceTest()
    {
        Directory.CreateDirectory(DirectoryPath);
    }
    
    [Fact]
    public async Task CleanupObjectStorageBackupAsyncTest()
    {
        const string path = $"{DirectoryPath}/v3.gzip";
        FileStream fileStream = File.Create(path);
        fileStream.Close();

        var file = new ObjectStorageFile
        {
            Name = "CleanupTest.txt",
            Path = "tests/CleanupTest.txt",
            BackupLocation = path,
            Status = SyncStatusEnum.Completed,
            SyncedAt = DateTime.UtcNow,
        };
        var instance = new ObjectStorageDeleteLog
        {
            DeletedAt = DateTime.UtcNow,
            File = file
        };
        DbContext.ObjectStorageDeleteLogs.Add(instance);
        await DbContext.SaveChangesAsync();

        var service = new CleanupService(
            dbContext: DbContext, 
            configuration: Configuration,
            logger: MockLogger<CleanupService>());
        await service.CleanupObjectStorageBackupAsync();
        Assert.False(File.Exists(path));
        Assert.NotNull(instance.ConfirmedAt);
        Assert.Equal(SyncStatusEnum.Deleted, file.Status);
    }
}
