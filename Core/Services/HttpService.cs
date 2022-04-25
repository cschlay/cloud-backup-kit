using System.Net;
using Core.Exceptions;

namespace Core.Services;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
   
    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<Stream> OpenHttpStreamAsync(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new HttpResourceDoesNotExist();
        }

        return await response.Content.ReadAsStreamAsync();
    }
}
