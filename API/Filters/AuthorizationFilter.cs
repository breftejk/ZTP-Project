using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using API.Services.Authorization;

namespace API.Filters;

/// <summary>
/// A filter that handles authorization using a Bearer token from the request headers.
/// </summary>
public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthorizationService _authorizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationFilter"/> class.
    /// </summary>
    /// <param name="authorizationService">The service used to authorize the user based on the token.</param>
    public AuthorizationFilter(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Executes the authorization logic for the current HTTP context.
    /// </summary>
    /// <param name="context">The context in which the authorization filter operates.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method checks for a valid Bearer token in the "Authorization" header.
    /// If the token is missing, invalid, or unauthorized, the request is rejected with a 401 Unauthorized status.
    /// </remarks>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Retrieve the Authorization header from the HTTP request
        var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Extract the token from the header
        var token = authHeader.Substring("Bearer ".Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            // Authorize the token and set the user principal
            var principal = await _authorizationService.AuthorizeAsync(token);
            context.HttpContext.User = principal;
        }
        catch
        {
            // If authorization fails, return 401 Unauthorized
            context.Result = new UnauthorizedResult();
        }
    }
}