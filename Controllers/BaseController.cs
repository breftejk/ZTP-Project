using Microsoft.AspNetCore.Mvc;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Base controller providing common functionalities for other controllers,
    /// such as session management and user identification.
    /// </summary>
    public class BaseController : Controller
    {
        // Key used for storing the selected language ID in the session.
        private const string SelectedLanguageSessionKey = "SelectedLanguage";

        /// <summary>
        /// Stores the selected language ID in the session for the current user.
        /// </summary>
        /// <param name="languageId">The ID of the selected language.</param>
        protected void SetSelectedLanguage(int languageId)
        {
            // Save the language ID in the session using the predefined key.
            HttpContext.Session.SetInt32(SelectedLanguageSessionKey, languageId);
        }

        /// <summary>
        /// Retrieves the selected language ID from the session.
        /// </summary>
        /// <returns>
        /// The selected language ID as an integer if it exists in the session;
        /// otherwise, null if the key is not set or the session has expired.
        /// </returns>
        protected int? GetSelectedLanguage()
        {
            // Retrieve the language ID from the session.
            return HttpContext.Session.GetInt32(SelectedLanguageSessionKey);
        }

        /// <summary>
        /// Retrieves the current user's unique identifier.
        /// </summary>
        /// <returns>
        /// The user ID as a string if the user is authenticated;
        /// otherwise, null.
        /// </returns>
        protected string? GetUserId()
        {
            // Extract the user ID from the authenticated user's claims.
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}