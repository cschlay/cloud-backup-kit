using System.Text.RegularExpressions;

namespace Core.Utils;

public class FileSanitizer : IFileSanitizer
{
    /// <inheritdoc />
    public Task<string> SanitizeFileNameAsync(string name)
    {
        string preformatted = Path.GetFileNameWithoutExtension(name).ToLower();
        string cleaned =  Regex.Replace(preformatted, @"[ \./;:<>\\\*%\$]", "");

        if (cleaned.Length == 0)
        {
            throw new ArgumentException("The file name is not safe!", nameof(name));
        }

        return Task.FromResult(cleaned);
    }

    /// <inheritdoc />
    public Task<string> SanitizePathAsync(string path)
    {
        throw new NotImplementedException();
    }
}