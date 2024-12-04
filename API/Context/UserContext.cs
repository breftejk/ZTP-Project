using API.Models;
using API.Services;

namespace API.Context;

/// <summary>
/// Provides functionality for managing a user's context, including their state and operations related to word sets.
/// </summary>
public class UserContext
{
    private readonly IWordSetService _wordSetService;

    /// <summary>
    /// Gets the user entity associated with this context.
/// </summary>
    public User User { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContext"/> class.
    /// </summary>
    /// <param name="user">The user entity to manage.</param>
    /// <param name="wordSetService">The service for managing word sets.</param>
    public UserContext(User user, IWordSetService wordSetService)
    {
        User = user;
        _wordSetService = wordSetService;
    }

    /// <summary>
    /// Retrieves all word sets associated with the current user.
/// </summary>
    /// <param name="includeWordPairs">A flag indicating whether to include word pairs in the result.</param>
    /// <returns>A list of word sets for the user.</returns>
    public List<WordSet> GetWordSets(bool includeWordPairs = false)
    {
        return _wordSetService.GetAllWordSets(User.Id, includeWordPairs);
    }

    /// <summary>
    /// Creates a new word set for the current user.
/// </summary>
    /// <param name="name">The name of the new word set.</param>
    /// <param name="languageCode">The language code of the new word set.</param>
    /// <returns>The created word set object.</returns>
    public WordSet CreateWordSet(string name, string languageCode)
    {
        return _wordSetService.CreateWordSet(name, languageCode, User.Id);
    }

    /// <summary>
    /// Adds a collection of word pairs to a specific word set belonging to the current user.
/// </summary>
    /// <param name="wordSetId">The unique identifier of the target word set.</param>
    /// <param name="wordPairIds">A list of word pair IDs to add to the word set.</param>
    public void AddWordPairsToSet(int wordSetId, List<int> wordPairIds)
    {
        _wordSetService.AddWordPairsToSet(wordSetId, wordPairIds, User.Id);
    }
}