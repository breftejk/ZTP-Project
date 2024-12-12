using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Learning.Challenges;
using ZTP_Project.Models;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for home and error-related pages.
    /// </summary>
    [Authorize]
    [LanguageSelected]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDailyChallengeService _dailyChallengeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="dailyChallengeService">Service for managing daily challenges.</param>
        /// <param name="logger">Logger instance.</param>
        public HomeController(DailyChallengeService dailyChallengeService, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dailyChallengeService = dailyChallengeService;
        }

        /// <summary>
        /// Displays the main page with the active daily challenge if available.
        /// </summary>
        /// <returns>The main page view.</returns>
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var languageId = GetSelectedLanguage();

            if (languageId.HasValue && !string.IsNullOrEmpty(userId))
            {
                var activeChallenge = await _dailyChallengeService.GetOrCreateDailyChallengeAsync(userId, languageId.Value);
                ViewBag.ActiveChallenge = activeChallenge;
            }
            else
            {
                ViewBag.ActiveChallenge = null;
            }

            return View();
        }

        /// <summary>
        /// Marks the specified daily challenge as completed.
        /// </summary>
        /// <param name="challengeId">The ID of the challenge to complete.</param>
        /// <returns>A redirect to the main page with a success message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteChallenge(int challengeId)
        {
            await _dailyChallengeService.CompleteChallengeAsync(challengeId);
            TempData["Success"] = "Daily Challenge marked as completed!";
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Handles application errors and displays the error page.
        /// </summary>
        /// <returns>The error page view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}