using Microsoft.AspNetCore.Mvc;
using API.Filters;

namespace API.Attributes;

/// <summary>
/// A custom attribute to apply the <see cref="AuthorizationFilter"/> for handling authorization in controllers or actions.
/// </summary>
/// <remarks>
/// This attribute leverages <see cref="TypeFilterAttribute"/> to enable dependency injection, ensuring that the 
/// <see cref="AuthorizationFilter"/> is executed as part of the request pipeline.
/// </remarks>
public class AuthorizationFilterAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Constructs a new instance of the <see cref="AuthorizationFilterAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// The base class constructor is invoked with the <see cref="AuthorizationFilter"/> type to attach it to the request pipeline.
    /// </remarks>
    public AuthorizationFilterAttribute() : base(typeof(AuthorizationFilter))
    {
    }
}