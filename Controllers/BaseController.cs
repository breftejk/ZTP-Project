using Microsoft.AspNetCore.Mvc;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Base controller providing common functionalities for other controllers, such as session management and user identification.
    /// </summary>
    public class BaseController : Controller
    {
        private const string SelectedLanguageSessionKey = "SelectedLanguage";

        /// <summary>
        /// Sets the selected language ID in the session.
        /// </summary>
        /// <param name="languageId">The ID of the selected language.</param>
        protected void SetSelectedLanguage(int languageId)
        {
            HttpContext.Session.SetInt32(SelectedLanguageSessionKey, languageId);
        }

        /// <summary>
        /// Retrieves the selected language ID from the session.
        /// </summary>
        /// <returns>The selected language ID or null if not set.</returns>
        protected int? GetSelectedLanguage()
        {
            return HttpContext.Session.GetInt32(SelectedLanguageSessionKey);
        }

        /// <summary>
        /// Retrieves the current user's unique identifier.
        /// </summary>
        /// <returns>The user ID or null if the user is not authenticated.</returns>
        protected string? GetUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}