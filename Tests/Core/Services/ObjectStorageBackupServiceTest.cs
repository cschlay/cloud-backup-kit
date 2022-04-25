using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Enums;
using Core.Models;
using Core.Services;
using Core.Utils;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.Core.Services;

public class ObjectStorageBackupServiceTest
{
    private readonly IConfiguration _configuration = TestConfiguration.BuildConfiguration();
    private const string DirectoryPath = $"{TestConstants.FileDirectoryRoot}/{TestConstants.StorageDirectory}";

    [Fact]
    public async Task BackupFileAsyncTest()
    {
        string content = $"test-{Guid.NewGuid()}";
        var file = new ObjectStorageFile
        {
            Name = "chewing-gum.jpg",
            Path = $"books/{nameof(BackupFileAsyncTest)}",
            SignedDownloadUrl = "http://localhost:5000/object-storage/url",
            Status = SyncStatusEnum.Pending,
            Storage = TestConstants.StorageDirectory,
            Version = 1
        };
        
        var mockHttpService = new Mock<IHttpService>();
        mockHttpService
            .Setup(m => m.OpenHttpStreamAsync(It.IsAny<string>()))
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        
        var service = new ObjectStorageBackupService(
            dbContext: new Mock<AppDbContext>().Object,
            configuration: _configuration,
            fileSanitizer: new FileSanitizer(),
            httpService: mockHttpService.Object);

        file = await service.BackupFileAsync(file);
        Assert.NotNull(file.SyncedAt);
        Assert.Equal(SyncStatusEnum.Completed, file.Status);

        var path = $"{DirectoryPath}/{file.Path.ToLower()}/v1.gzip";
        Assert.Equal(path, file.BackupLocation);
    }

    [Fact]
    public async Task SaveFileAsyncTest()
    {
        string content = $"test-{Guid.NewGuid()}";
        var mockHttpService = new Mock<IHttpService>();
        mockHttpService
            .Setup(m => m.OpenHttpStreamAsync(It.IsAny<string>()))
            .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        
        var service = new ObjectStorageBackupService(
            dbContext: new Mock<AppDbContext>().Object,
            configuration: _configuration,
            fileSanitizer: new FileSanitizer(),
            httpService: mockHttpService.Object);

        const string filename = $"{nameof(SaveFileAsyncTest)}.txt.gzip";
        const string path = $"{DirectoryPath}/{filename}";
        await service.SaveFileAsync("/", DirectoryPath, filename);
        
        string savedContent = await FileTools.ReadGzipTextContentAsync(path);
        Assert.True(File.Exists(path));
        Assert.Equal(content, savedContent);
    }

    [Fact]
    public async Task SaveFileAsync404Test()
    {
        throw new NotImplementedException();
    }
}
