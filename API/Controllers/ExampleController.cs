using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using API.Filters;

namespace API.Controllers;

[ApiController]
[Route("api/example")]
public class ExampleController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult GetPublicData()
    {
        return Ok("This is public data.");
    }

    [HttpGet("secured")]
    [AuthorizationFilter()]
    public IActionResult GetSecuredData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            Id = userId,
            Email = userEmail
        });
    }
}