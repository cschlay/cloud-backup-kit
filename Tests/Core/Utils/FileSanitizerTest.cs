using System;
using System.Threading.Tasks;
using Core.Utils;
using Xunit;

namespace Tests.Core.Utils;

public class FileSanitizerTest
{
    private readonly FileSanitizer _sanitizer;
    
    public FileSanitizerTest()
    {
        _sanitizer = new FileSanitizer();
    }

    [Theory]
    [InlineData("FileName.sh", "filename")]
    [InlineData("FileName", "filename")]
    [InlineData("cde69a13-38f9-4d2a-b9b1-dec5c54cb031.png", "cde69a13-38f9-4d2a-b9b1-dec5c54cb031")]
    [InlineData("../../../name.sh", "name")]
    [InlineData("/var/www/index.php", "index")]
    [InlineData("/;:<>/\\test.*%$.ps1", "test")]
    public async Task SanitizeFileNameAsyncTest(string name, string expected)
    {
        string result = await _sanitizer.SanitizeFileNameAsync(name);
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(".")]
    [InlineData("..")]
    [InlineData(".............")]
    [InlineData("....jpg")]
    public async Task SanitizeFileNameAsyncInvalidTest(string name)
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await _sanitizer.SanitizeFileNameAsync(name));
    }

    [Theory]
    [InlineData("/06fd9f95-f8e6-401c-b435-0253a3527270", "06fd9f95-f8e6-401c-b435-0253a3527270")]
    [InlineData("/a/b/c/", "a/b/c")]
    [InlineData("a", "a")]
    [InlineData("/a", "a")]
    public async Task ValidatePathAsyncTest(string path, string expected)
    {
        string result = await _sanitizer.SanitizePathAsync(path);
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("../../cars")]
    [InlineData("../cars/../models")]
    [InlineData("")]
    [InlineData(".")]
    [InlineData("./")]
    [InlineData("..")]
    [InlineData("///")]
    [InlineData("/var bash")]
    public async Task ValidatePathAsyncInvalidTest(string path)
    {
        await Assert.ThrowsAsync<ArgumentException>(async () => await _sanitizer.SanitizePathAsync(path));
    }
}
