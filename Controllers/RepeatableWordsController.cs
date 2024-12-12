using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Learning.RepeatableWords;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for handling repeatable words for users.
    /// </summary>
    [Authorize]
    [LanguageSelected]
    public class RepeatableWordsController : BaseController
    {
        private readonly RepeatableWordsService _repeatableWordsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatableWordsController"/> class.
        /// </summary>
        /// <param name="repeatableWordsService">Service for managing repeatable words.</param>
        public RepeatableWordsController(RepeatableWordsService repeatableWordsService)
        {
            _repeatableWordsService = repeatableWordsService;
        }

        /// <summary>
        /// Displays the list of words that the user needs to repeat.
        /// </summary>
        /// <returns>A view with the list of repeatable words.</returns>
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var languageId = GetSelectedLanguage();

            if (string.IsNullOrEmpty(userId) || !languageId.HasValue)
            {
                TempData["Error"] = "Please log in and select a language.";
                return RedirectToAction("SelectLanguage", "LanguageSelection");
            }

            var wordsToRepeat = await _repeatableWordsService.GetWordsToRepeatAsync(userId, languageId.Value);
            return View(wordsToRepeat);
        }
    }
}