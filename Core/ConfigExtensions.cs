using Microsoft.Extensions.Configuration;

namespace Core;

public static class ConfigExtensions
{
    public static string GetTempFileRoot(this IConfiguration conf) => conf["App:TempFileRoot"];
}
