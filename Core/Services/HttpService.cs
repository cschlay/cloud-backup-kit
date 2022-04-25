namespace Core.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
   
    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<Stream> OpenHttpStreamAsync(string url) => await _httpClient.GetStreamAsync(url);
}
