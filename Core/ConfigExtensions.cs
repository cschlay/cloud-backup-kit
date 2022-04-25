﻿using Microsoft.Extensions.Configuration;

namespace Core;

public static class ConfigExtensions
{
    public static string GetFileSystemRoot(this IConfiguration conf) => conf["App:FileSystemRoot"];

    public static int GetObjectStorageSyncInterval(this IConfiguration conf) => int.Parse(conf["Intervals:ObjectStorage"]);
}