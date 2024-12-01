using Microsoft.AspNetCore.Mvc;
using API.Filters;

namespace API.Attributes;

/// <summary>
/// A custom attribute for applying the <see cref="AuthorizationFilter"/> to controllers or actions.
/// </summary>
/// <remarks>
/// This attribute uses <see cref="TypeFilterAttribute"/> to enable dependency injection for the filter.
/// It ensures that the <see cref="AuthorizationFilter"/> is executed during the request pipeline to handle authorization.
/// </remarks>
public class AuthorizationFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationFilterAttribute"/> class.
    /// </summary>
    public AuthorizationFilterAttribute() : base(typeof(AuthorizationFilter))
    {
    }
}