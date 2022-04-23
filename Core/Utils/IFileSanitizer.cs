namespace Core.Utils;

public interface IFileSanitizer
{
    /// <summary>
    /// Sanitizes the file name, it should likely be an uuid or a hash already.
    /// </summary>
    /// <param name="name">of the file</param>
    /// <exception cref="ArgumentException">the filename is not safe to use</exception>
    /// <returns>sanitized name</returns>
    public Task<string> SanitizeFileNameAsync(string name);
    
    /// <summary>
    /// Strips all suspicious elements from path.
    /// </summary>
    /// <param name="path">the path</param>
    /// <exception cref="ArgumentException">if the path is likely an injection</exception>
    /// <returns>Sanitized path name</returns>
    public Task<string> SanitizePathAsync(string path);
}
