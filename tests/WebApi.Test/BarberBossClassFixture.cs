using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class BarberBossClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public BarberBossClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    public async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizationRequest(token);
        SetCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    public async Task<HttpResponseMessage> DoGet(string requestUri, string token = "", string culture = "en")
    {
        AuthorizationRequest(token);
        SetCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }
    public async Task<HttpResponseMessage> DoDelte(string requestUri, string token, string culture = "en")
    {
        AuthorizationRequest(token);
        SetCulture(culture);

        return await _httpClient.DeleteAsync(requestUri);
    }

    public async Task<HttpResponseMessage> DoPut(string requestUri, object request, string token, string culture = "en")
    {
        AuthorizationRequest(token);
        SetCulture(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    private void AuthorizationRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void SetCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
