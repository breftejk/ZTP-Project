using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers;

/// <summary>
/// API Controller for managing word pairs and translations.
/// </summary>
[ApiController]
[Route("api/words")]
public class WordController : ControllerBase
{
    private readonly WordFacade _wordFacade;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordController"/> class.
    /// </summary>
    /// <param name="wordFacade">The facade used for accessing word-related operations.</param>
    public WordController(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    /// <summary>
    /// Retrieves all word pairs for a specific language.
    /// </summary>
    /// <param name="languageCode">
    /// The language code for which to retrieve word pairs (e.g., "en", "de").
    /// Use "all" to retrieve word pairs from all languages.
    /// </param>
    /// <returns>
    /// A list of word pairs for the specified language.
    /// If "all" is used, word pairs from all languages are returned.
    /// </returns>
    /// <response code="200">Returns the list of word pairs.</response>
    /// <response code="400">If the specified language is not supported.</response>
    [HttpGet("{languageCode}/all")]
    [ProducesResponseType(typeof(List<WordPair>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult GetAllWordPairs(string languageCode)
    {
        try
        {
            var words = _wordFacade.GetAllWords(languageCode);
            return Ok(words);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves translations for a specific word in a given language.
    /// </summary>
    /// <param name="languageCode">The language code (e.g., "en", "de").</param>
    /// <param name="word">The word to translate.</param>
    /// <returns>A list of translations for the specified word.</returns>
    /// <response code="200">Returns the list of translations.</response>
    /// <response code="400">If the specified language is not supported.</response>
    /// <response code="404">If no translations are found for the word in the specified language.</response>
    [HttpGet("{languageCode}/translate/{word}")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public IActionResult GetTranslations(string languageCode, string word)
    {
        try
        {
            var translations = _wordFacade.GetTranslations(word, languageCode);
            if (translations.Count == 0)
            {
                return NotFound($"No translations found for '{word}' in language '{languageCode}'.");
            }

            return Ok(translations);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds a new word pair for a specific language.
    /// </summary>
    /// <param name="languageCode">The language code for the word pair (e.g., "en", "de").</param>
    /// <param name="request">The word pair to add.</param>
    /// <returns>A success message if the word pair is added successfully.</returns>
    /// <response code="200">If the word pair is added successfully.</response>
    /// <response code="400">If the specified language is not supported.</response>
    [HttpPost("{languageCode}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult AddWordPair(string languageCode, [FromBody] AddWordPairRequest request)
    {
        try
        {
            _wordFacade.AddWordPair(request.Word, request.Translation, languageCode);
            return Ok("Word pair added.");
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/// <summary>
/// Request model for adding a new word pair.
/// </summary>
public class AddWordPairRequest
{
    /// <summary>
    /// The source word.
    /// </summary>
    public string Word { get; set; }

    /// <summary>
    /// The translation of the word.
    /// </summary>
    public string Translation { get; set; }
}