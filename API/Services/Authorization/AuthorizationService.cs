using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace API.Services.Authorization;

/// <summary>
/// A service for authorizing users using a Bearer token and retrieving user information from an external endpoint.
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private const string UserInfoEndpoint = "https://ztp-project.eu.auth0.com/userinfo";

    /// <summary>
    /// Authorizes a user based on the provided token and retrieves user information from the external user info endpoint.
    /// </summary>
    /// <param name="token">The Bearer token used for authorization.</param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> representing the authenticated user with claims populated from the user info endpoint.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the token is invalid, the external endpoint returns an error, or the user information is null.
    /// </exception>
    public async Task<ClaimsPrincipal> AuthorizeAsync(string token)
    {
        // Create an HTTP client to communicate with the user info endpoint
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Make a GET request to the user info endpoint
        var response = await client.GetAsync(UserInfoEndpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("Failed to retrieve user info.");
        }

        // Parse the response content
        var content = await response.Content.ReadAsStringAsync();
        var userInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

        if (userInfo == null)
        {
            throw new UnauthorizedAccessException("User info is null.");
        }

        // Create claims based on the retrieved user information
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo["sub"].ToString()),
            new Claim(ClaimTypes.Email, userInfo["email"].ToString())
        };

        // Create and return a ClaimsPrincipal representing the authenticated user
        var identity = new ClaimsIdentity(claims, "Bearer");
        return new ClaimsPrincipal(identity);
    }
}