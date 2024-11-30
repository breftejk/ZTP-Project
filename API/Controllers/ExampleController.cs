using Microsoft.AspNetCore.Mvc;
using API.Filters;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult GetPublicData()
    {
        return Ok("This is public data.");
    }

    [HttpGet("secured")]
    [AuthorizationFilter(
        audience: "your-api-identifier",
        issuer: "https://ztp-project.eu.auth0.com/",
        secret: "your-auth0-secret-key")]
    public IActionResult GetSecuredData()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        return Ok($"This is secured data for user {userId}.");
    }
}