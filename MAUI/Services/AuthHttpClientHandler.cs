namespace MAUI.Services;

public class AuthHttpClientHandler : DelegatingHandler
{
    private readonly TokenManager _tokenManager;

    public AuthHttpClientHandler(TokenManager tokenManager, HttpMessageHandler innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        _tokenManager = tokenManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenManager.GetAccessTokenAsync();

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}