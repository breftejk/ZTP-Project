using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers;

[ApiController]
[Route("api/words")]
public class WordController : ControllerBase
{
    private readonly WordFacade _wordFacade;

    public WordController(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    [HttpGet("{language}/all")]
    public IActionResult GetAllWordPairs(string language)
    {
        try
        {
            var words = _wordFacade.GetAllWords(language);
            return Ok(words);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{language}/translate/{word}")]
    public IActionResult GetTranslations(string language, string word)
    {
        try
        {
            var translations = _wordFacade.GetTranslations(word, language);
            if (translations.Count == 0)
            {
                return NotFound($"No translations found for '{word}' in language '{language}'.");
            }

            return Ok(translations);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{language}")]
    public IActionResult AddWordPair(string language, [FromBody] AddWordPairRequest request)
    {
        try
        {
            _wordFacade.AddWordPair(request.Word, request.Translation, language);
            return Ok("Word pair added.");
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class AddWordPairRequest
{
    public string Word { get; set; }
    public string Translation { get; set; }
}