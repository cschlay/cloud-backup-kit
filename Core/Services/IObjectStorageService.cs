﻿using Core.Models;

namespace Core.Services;

public interface IObjectStorageService
{
    /// <summary>
    /// Downloads the file from temporary cloud storage.
    /// </summary>
    /// <param name="file">information of the file</param>
    /// <returns>The temporary location stored</returns>
    public Task<string> DownloadFileAsync(ObjectStorageFile file);
}

