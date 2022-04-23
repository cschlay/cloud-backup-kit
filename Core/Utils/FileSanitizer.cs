namespace Core.Utils;

public class FileSanitizer : IFileSanitizer
{
    /// <inheritdoc />
    public Task<string> SanitizeFileNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<string> SanitizePathAsync(string path)
    {
        throw new NotImplementedException();
    }
}