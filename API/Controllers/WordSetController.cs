using System.Security.Claims;
using API.Attributes;
using API.Context;
using API.Factories;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller responsible for handling operations related to word sets for authenticated users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WordSetController : ControllerBase
{
    private readonly UserFactory _userFactory;
    private readonly IWordSetService _wordSetService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordSetController"/> class.
    /// </summary>
    /// <param name="userFactory">Factory for creating or retrieving user instances.</param>
    /// <param name="wordSetService">Service to manage word set operations.</param>
    public WordSetController(UserFactory userFactory, IWordSetService wordSetService)
    {
        _userFactory = userFactory;
        _wordSetService = wordSetService;
    }

    /// <summary>
    /// Retrieves all word sets associated with the authenticated user.
    /// </summary>
    /// <param name="includeWordPairs">A flag to specify whether to include word pairs in the response.</param>
    /// <returns>A list of word sets for the user.</returns>
    [HttpGet("wordsets")]
    [AuthorizationFilter]
    public IActionResult GetWordSets([FromQuery] bool includeWordPairs = false)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { Message = "User ID not found in the authentication token." });
        }

        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        var wordSets = userContext.GetWordSets(includeWordPairs);
        return Ok(wordSets);
    }

    /// <summary>
    /// Creates a new word set for the authenticated user.
    /// </summary>
    /// <param name="request">Details of the word set to be created.</param>
    /// <returns>The newly created word set.</returns>
    [HttpPost("wordsets")]
    [AuthorizationFilter]
    public IActionResult CreateWordSet([FromBody] WordSetCreationRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { Message = "User ID not found in the authentication token." });
        }

        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        var wordSet = userContext.CreateWordSet(request.Name, request.LanguageCode);
        return CreatedAtAction(nameof(GetWordSets), null, wordSet);
    }

    /// <summary>
    /// Adds a collection of word pairs to a specified word set owned by the authenticated user.
    /// </summary>
    /// <param name="wordSetId">The unique identifier of the target word set.</param>
    /// <param name="wordPairIds">A list of word pair IDs to add to the word set.</param>
    /// <returns>No content if the operation is successful.</returns>
    [HttpPost("wordsets/{wordSetId}/add-pairs")]
    [AuthorizationFilter]
    public IActionResult AddWordPairsToSet(int wordSetId, [FromBody] List<int> wordPairIds)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { Message = "User ID not found in the authentication token." });
        }

        var user = _userFactory.GetOrCreateUser(userId);
        var userContext = new UserContext(user, _wordSetService);

        userContext.AddWordPairsToSet(wordSetId, wordPairIds);
        return NoContent();
    }
}

/// <summary>
/// Represents the request payload for creating a new word set.
/// </summary>
public class WordSetCreationRequest
{
    /// <summary>
    /// Gets or sets the name of the word set to be created.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the language code for the word set.
    /// </summary>
    public string LanguageCode { get; set; }
}