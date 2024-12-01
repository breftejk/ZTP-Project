using System.Security.Claims;

namespace API.Services;

/// <summary>
/// A decorator for the <see cref="IAuthorizationService"/> that adds logging functionality to the authorization process.
/// </summary>
public class AuthorizationServiceDecorator : IAuthorizationService
{
    private readonly IAuthorizationService _inner;
    private readonly ILogger<AuthorizationServiceDecorator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationServiceDecorator"/> class.
    /// </summary>
    /// <param name="inner">The inner <see cref="IAuthorizationService"/> to which authorization requests are delegated.</param>
    /// <param name="logger">The logger used to log information about the authorization process.</param>
    public AuthorizationServiceDecorator(IAuthorizationService inner, ILogger<AuthorizationServiceDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    /// <summary>
    /// Authorizes a user based on the provided token and logs the process.
    /// </summary>
    /// <param name="token">The token used to authenticate and authorize the user.</param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> representing the authenticated user if the token is valid.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the underlying <see cref="IAuthorizationService"/> fails to authorize the user.
    /// </exception>
    public async Task<ClaimsPrincipal> AuthorizeAsync(string token)
    {
        _logger.LogInformation("Starting authorization process.");

        var principal = await _inner.AuthorizeAsync(token);

        _logger.LogInformation($"Authorization successful for user: {principal.Identity.Name}");
        
        return principal;
    }
}