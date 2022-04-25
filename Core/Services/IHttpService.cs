namespace Core.Services;

public interface IHttpService
{
    public Task<Stream> OpenHttpStreamAsync(string url);
}
