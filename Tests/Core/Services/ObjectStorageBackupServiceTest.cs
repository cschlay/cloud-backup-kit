using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Services;
using Core.Utils;
using Moq;
using Xunit;

namespace Tests.Core.Services;

public class ObjectStorageBackupServiceTest
{
    [Fact]
    public async Task BackupFileAsyncTest()
    {
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
            new Mock<AppDbContext>().Object,
            new FileSanitizer(),
            mockHttpService.Object);

        const string path = $"{TestConstants.FileDirectoryRoot}/{nameof(SaveFileAsyncTest)}.txt.gzip";
        await service.SaveFileAsync("/", path);
        Assert.True(File.Exists(path));

        string savedContent = await FileTools.ReadGzipTextContentAsync(path);
        Assert.Equal(content, savedContent);
    }
}
