using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        private const string UserInfoEndpoint = "https://ztp-project.eu.auth0.com/userinfo";

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            try
            {
                var userInfo = await GetUserInfoAsync(token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo["sub"].ToString()),
                    new Claim(ClaimTypes.Email, userInfo["email"].ToString())
                };

                var identity = new ClaimsIdentity(claims, "Bearer");
                var principal = new ClaimsPrincipal(identity);
                context.HttpContext.User = principal;
            }
            catch
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }

        private async Task<Dictionary<string, object>> GetUserInfoAsync(string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(UserInfoEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException();
            }

            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

            if (userInfo == null)
            {
                throw new UnauthorizedAccessException();
            }

            return userInfo;
        }
    }
}