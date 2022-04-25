using System.IO.Compression;
using System.Text;

namespace Core.Utils;

/// <summary>
/// Utilities to handle file operations such as compression
/// </summary>
public static class FileTools
{
    /// <summary>
    /// Compress the file as using Gzip-algorithm.
    /// </summary>
    /// <param name="source">to compress</param>
    /// <param name="target">path for the result</param>
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.gzipstream"/>
    public static async void CompressAsync(Stream source, Stream target)
    {
        await using var gzip = new GZipStream(target, CompressionLevel.SmallestSize);
        await source.CopyToAsync(gzip);
    }

    public static async Task<string> ReadGzipTextContentAsync(string filepath)
    {
        await using FileStream source = File.Open(filepath, FileMode.Open);
        await using var result = new MemoryStream();
        await using var gzip = new GZipStream(source, CompressionMode.Decompress);
        await gzip.CopyToAsync(result);

        return Encoding.UTF8.GetString(result.ToArray());
    }
}
