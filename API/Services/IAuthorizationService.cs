using System.Security.Claims;

namespace API.Services;

/// <summary>
/// Defines the contract for an authorization service that validates a Bearer token and retrieves user claims.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Authorizes a user based on the provided Bearer token.
    /// </summary>
    /// <param name="token">The Bearer token used for authorization.</param>
    /// <returns>
    /// A <see cref="ClaimsPrincipal"/> representing the authenticated user with claims populated from the token.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the token is invalid or authorization fails.
    /// </exception>
    Task<ClaimsPrincipal> AuthorizeAsync(string token);
}