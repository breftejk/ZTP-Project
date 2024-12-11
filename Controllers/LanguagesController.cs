using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Models;
using ZTP_Project.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for managing languages.
    /// </summary>
    public class LanguagesController : Controller
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguagesController(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
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
        public async Task<IActionResult> Create(Language language)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(language);
            }

            try
            {
                await _languageRepository.AddAsync(language);
                await _languageRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View(language);
            }
        }
    }
}