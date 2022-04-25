namespace Core.Services.Interfaces;

public interface IHttpService
{
    public Task<Stream> OpenHttpStreamAsync(string url);
}
