using API.Context;
using API.Factories;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserFactory _userFactory;
    private readonly IWordSetService _wordSetService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userFactory">Factory for retrieving or creating users.</param>
    /// <param name="wordSetService">Service for managing word sets.</param>
    public UserController(UserFactory userFactory, IWordSetService wordSetService)
    {
        _userFactory = userFactory;
        _wordSetService = wordSetService;
    }

    /// <summary>
    /// Retrieves all word sets for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="includeWordPairs">Whether to include word pairs in the result.</param>
    /// <returns>A list of word sets.</returns>
    [HttpGet("{userId}/wordsets")]
    public IActionResult GetWordSets(string userId, [FromQuery] bool includeWordPairs = false)
    {
        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        var wordSets = userContext.GetWordSets(includeWordPairs);
        return Ok(wordSets);
    }

    /// <summary>
    /// Creates a new word set for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="name">The name of the word set.</param>
    /// <param name="languageCode">The language code of the word set.</param>
    /// <returns>The created word set.</returns>
    [HttpPost("{userId}/wordsets")]
    public IActionResult CreateWordSet(string userId, [FromBody] WordSetCreationRequest request)
    {
        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        var wordSet = userContext.CreateWordSet(request.Name, request.LanguageCode);
        return CreatedAtAction(nameof(GetWordSets), new { userId }, wordSet);
    }

    /// <summary>
    /// Adds word pairs to a user's word set.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="wordSetId">The ID of the word set.</param>
    /// <param name="wordPairIds">The list of word pair IDs to add.</param>
    [HttpPost("/wordsets/{wordSetId}/add-pairs")]
    public IActionResult AddWordPairsToSet(string userId, int wordSetId, [FromBody] List<int> wordPairIds)
    {
        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        userContext.AddWordPairsToSet(wordSetId, wordPairIds);
        return NoContent();
    }
}

/// <summary>
/// Request model for creating a word set.
/// </summary>
public class WordSetCreationRequest
{
    public string Name { get; set; }
    public string LanguageCode { get; set; }
}