using System.Security.Claims;
using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// A sample controller demonstrating public and secured endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    /// <summary>
    /// Retrieves public data that does not require authentication.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing public data as a string.
    /// </returns>
    [HttpGet("public")]
    public IActionResult GetPublicData()
    {
        return Ok("This is public data.");
    }

    /// <summary>
    /// Retrieves secured data that requires authorization.
    /// </summary>
    /// <remarks>
    /// This endpoint uses the <see cref="AuthorizationFilterAttribute"/> to enforce Bearer token-based authorization.
    /// </remarks>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the authenticated user's ID and email address.
    /// </returns>
    [HttpGet("secured")]
    [AuthorizationFilter]
    public IActionResult GetSecuredData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;

        return Ok(new
        {
            Id = userId,
            Email = userEmail
        });
    }
}