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
        bool isAlphanumeric = Regex.IsMatch(path, @"[a-zA-Z0-9_\-/]+");
        bool hasInvalidPatterns = Regex.IsMatch(path, @"(//)|\.|\s");
        if (!isAlphanumeric || hasInvalidPatterns)
        {
            throw new ArgumentException("The path is not safe!", nameof(path));
        }

        string sanitized = path;
        if (path.EndsWith('/'))
        {
            sanitized = path[..^1]; // Exclude the last slash
        }

        string result = !path.StartsWith('/') ? $"/{sanitized}" : sanitized;
        return Task.FromResult(result.ToLower());
    }
}
