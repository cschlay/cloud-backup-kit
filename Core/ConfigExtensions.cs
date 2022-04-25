using Microsoft.Extensions.Configuration;

namespace Core;

public static class ConfigExtensions
{
    public static string GetFileSystemRoot(this IConfiguration conf) => conf["App:FileSystemRoot"];
}
