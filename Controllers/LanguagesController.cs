using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZTP_Project.Models;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for managing languages.
    /// </summary>
    public class LanguagesController : BaseController
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguagesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesController"/> class.
        /// </summary>
        /// <param name="languageRepository">Repository for language operations.</param>
        /// <param name="logger">Logger instance.</param>
        public LanguagesController(ILanguageRepository languageRepository, ILogger<LanguagesController> logger)
        {
            _languageRepository = languageRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all languages.
        /// </summary>
        /// <returns>A view with the list of languages.</returns>
        public async Task<IActionResult> Index()
        {
            var languages = await _languageRepository.GetAllAsync();
            return View(languages);
        }

        /// <summary>
        /// Displays a form for creating a new language.
        /// </summary>
        /// <returns>A view with the form to create a language.</returns>
        [HttpGet]
        public IActionResult Create() => View();

        /// <summary>
        /// Handles the creation of a new language.
        /// </summary>
        /// <param name="language">The language to create.</param>
        /// <returns>A redirect to the list of languages or an error view if creation fails.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Language language)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation error: {error.ErrorMessage}");
                }
                return View(language);
            }

            try
            {
                await _languageRepository.AddAsync(language);
                await _languageRepository.SaveChangesAsync();
                TempData["Success"] = "Language created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating language.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the language.");
                return View(language);
            }
        }
    }
}