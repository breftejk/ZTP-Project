using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ZTP_Project.Factories;
using ZTP_Project.Models;
using ZTP_Project.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for managing words and their import/export functionality.
    /// </summary>
    public class WordsController : Controller
    {
        private readonly IWordRepository _wordRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IExporterFactory _exporterFactory;
        private readonly IImporterFactory _importerFactory;

        public WordsController(
            IWordRepository wordRepository,
            ILanguageRepository languageRepository,
            IExporterFactory exporterFactory,
            IImporterFactory importerFactory)
        {
            _wordRepository = wordRepository;
            _languageRepository = languageRepository;
            _exporterFactory = exporterFactory;
            _importerFactory = importerFactory;
        }

        /// <summary>
        /// Displays a list of all words.
        /// </summary>
        /// <returns>A view with the list of words.</returns>
        public async Task<IActionResult> Index()
        {
            var words = await _wordRepository.FindAsync(w => true);
            var languages = await _languageRepository.GetAllAsync();
            foreach (var word in words)
            {
                word.Language = languages.FirstOrDefault(l => l.Id == word.LanguageId);
            }
            return View(words);
        }

        /// <summary>
        /// Displays a form for creating a new word.
        /// </summary>
        /// <returns>A view with the form to create a word.</returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Languages = await _languageRepository.GetAllAsync();
            return View();
        }

        /// <summary>
        /// Handles the creation of a new word.
        /// </summary>
        /// <param name="word">The word to create.</param>
        /// <returns>A redirect to the list of words or an error view if creation fails.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Word word)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                ViewBag.Languages = await _languageRepository.GetAllAsync();
                return View(word);
            }

            try
            {
                await _wordRepository.AddAsync(word);
                await _wordRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ViewBag.Languages = await _languageRepository.GetAllAsync();
                return View(word);
            }
        }

        /// <summary>
        /// Displays the export data page.
        /// </summary>
        /// <returns>A view with the export options.</returns>
        [HttpGet]
        public IActionResult ExportData() => View();

        /// <summary>
        /// Handles data export in the specified format.
        /// </summary>
        /// <param name="format">The format for export (e.g., JSON, XML, CSV).</param>
        /// <returns>A file containing the exported data.</returns>
        [HttpPost]
        public async Task<IActionResult> ExportData(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                TempData["Error"] = "Please select a format for export.";
                return RedirectToAction("Export");
            }

            try
            {
                var words = await _wordRepository.FindAsync(w => true);
                var languages = await _languageRepository.GetAllAsync();
                foreach (var word in words)
                {
                    word.Language = languages.FirstOrDefault(l => l.Id == word.LanguageId);
                }

                var exporter = _exporterFactory.GetExporter<Word>(format);
                var fileContent = exporter.Export(words);
                var fileName = $"Words_{DateTime.Now:yyyyMMddHHmmss}{exporter.FileExtension}";

                return File(fileContent, exporter.ContentType, fileName);
            }
            catch (NotSupportedException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Export");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during export.";
                return RedirectToAction("Export");
            }
        }

        /// <summary>
        /// Displays the import data page.
        /// </summary>
        /// <returns>A view with the import options.</returns>
        [HttpGet]
        [Authorize(Policy = "ImportPolicy")]
        public IActionResult ImportData() => View();

        /// <summary>
        /// Handles data import from the specified file.
        /// </summary>
        /// <param name="format">The format of the imported data (e.g., JSON, XML, CSV).</param>
        /// <param name="file">The uploaded file containing data to import.</param>
        /// <returns>A redirect to the list of words or an error view if import fails.</returns>
        [HttpPost]
        [Authorize(Policy = "ImportPolicy")]
        public async Task<IActionResult> ImportData(string format, IFormFile file)
        {
            if (string.IsNullOrEmpty(format))
            {
                TempData["Error"] = "Please select a format for import.";
                return RedirectToAction("ImportData");
            }

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid file.";
                return RedirectToAction("ImportData");
            }

            try
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                var importer = _importerFactory.GetImporter<Word>(format);
                var importedWords = importer.Import(fileData);
                var languages = await _languageRepository.GetAllAsync();

                var wordsToAdd = new List<Word>();
                var errors = new List<string>();
                int lineNumber = 1;

                foreach (var word in importedWords)
                {
                    var validationContext = new ValidationContext(word, null, null);
                    var validationResults = new List<ValidationResult>();
                    bool isValid = Validator.TryValidateObject(word, validationContext, validationResults, true);

                    if (!isValid)
                    {
                        foreach (var validationResult in validationResults)
                        {
                            errors.Add($"Entry {lineNumber}: {validationResult.ErrorMessage}");
                        }
                    }
                    else if (!languages.Any(l => l.Id == word.LanguageId))
                    {
                        errors.Add($"Entry {lineNumber}: LanguageId {word.LanguageId} does not exist.");
                    }
                    else
                    {
                        wordsToAdd.Add(word);
                    }

                    lineNumber++;
                }

                if (errors.Any())
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("ImportData");
                }

                await _wordRepository.AddRangeAsync(wordsToAdd);
                await _wordRepository.SaveChangesAsync();

                TempData["Success"] = $"{wordsToAdd.Count} words imported successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (NotSupportedException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ImportData");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = $"Import failed: {ex.Message}";
                return RedirectToAction("ImportData");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred during import.";
                return RedirectToAction("ImportData");
            }
        }
    }
}