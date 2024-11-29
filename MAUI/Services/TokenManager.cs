using System.IdentityModel.Tokens.Jwt;
using Auth0.OidcClient;

namespace MAUI.Services;

public class TokenManager
{
    private readonly Auth0Client _auth0Client;
    private string _accessToken;
    private string _refreshToken;

    public TokenManager(Auth0Client auth0Client)
    {
        _auth0Client = auth0Client;
    }

    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;

        if (!string.IsNullOrEmpty(accessToken))
            await SecureStorage.SetAsync("access_token", accessToken);

        if (!string.IsNullOrEmpty(refreshToken))
            await SecureStorage.SetAsync("refresh_token", refreshToken);
    }

    private async Task LoadTokensAsync()
    {
        _accessToken = await SecureStorage.GetAsync("access_token");
        _refreshToken = await SecureStorage.GetAsync("refresh_token");
    }

    public async Task<string> GetAccessTokenAsync()
    {
        await LoadTokensAsync();

        if (string.IsNullOrEmpty(_accessToken) || IsTokenExpired(_accessToken))
        {
            await RefreshTokensAsync();
        }

        return _accessToken;
    }

    private async Task RefreshTokensAsync()
    {
        if (string.IsNullOrEmpty(_refreshToken))
        {
            throw new Exception("Refresh token is missing. Please log in again.");
        }

        var refreshResult = await _auth0Client.RefreshTokenAsync(_refreshToken);

        if (refreshResult.IsError)
        {
            throw new Exception($"Failed to refresh token: {refreshResult.Error}");
        }

        await SetTokensAsync(refreshResult.AccessToken, refreshResult.RefreshToken);
    }

    private bool IsTokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;
            return expiration < DateTime.UtcNow.AddMinutes(-5);
        }

        throw new ArgumentException("Invalid JWT token");
    }
}