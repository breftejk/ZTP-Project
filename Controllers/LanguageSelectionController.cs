using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for handling language selection.
    /// </summary>
    [Authorize]
    public class LanguageSelectionController : BaseController
    {
        private readonly ILanguageRepository _languageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSelectionController"/> class.
        /// </summary>
        /// <param name="languageRepository">Repository for language operations.</param>
        public LanguageSelectionController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        /// <summary>
        /// Displays the language selection page.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after language selection.</param>
        /// <returns>A view with the list of languages.</returns>
        [HttpGet]
        public async Task<IActionResult> SelectLanguage(string returnUrl)
        {
            var languages = await _languageRepository.GetAllAsync();
            ViewBag.ReturnUrl = returnUrl;
            return View(languages);
        }

        /// <summary>
        /// Sets the selected language and redirects to the specified URL.
        /// </summary>
        /// <param name="languageId">The ID of the selected language.</param>
        /// <param name="returnUrl">The URL to redirect to after language selection.</param>
        /// <returns>A redirect to the return URL or the home page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(int languageId, string returnUrl)
        {
            SetSelectedLanguage(languageId);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}