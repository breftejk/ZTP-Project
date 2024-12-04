using API.Models;

namespace API.Services.Word.Sets;

/// <summary>
/// Interface for the WordSetService, providing methods to manage word sets.
/// </summary>
public interface IWordSetService
{
    /// <summary>
    /// Creates a new word set with a specific language code for a user.
    /// </summary>
    /// <param name="name">The name of the word set.</param>
    /// <param name="languageCode">The language code of the word set.</param>
    /// <param name="userId">The ID of the user creating the word set.</param>
    /// <returns>The created word set.</returns>
    WordSet CreateWordSet(string name, string languageCode, string userId);

    /// <summary>
    /// Adds word pairs to a user's word set, ensuring they match the set's language code.
    /// </summary>
    /// <param name="wordSetId">The ID of the word set.</param>
    /// <param name="wordPairIds">The list of word pair IDs to add.</param>
    /// <param name="userId">The ID of the user owning the word set.</param>
    void AddWordPairsToSet(int wordSetId, List<int> wordPairIds, string userId);

    /// <summary>
    /// Retrieves all word sets for a user, optionally including word pairs.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="includeWordPairs">Whether to include word pairs in the result.</param>
    /// <returns>A list of word sets.</returns>
    List<WordSet> GetAllWordSets(string userId, bool includeWordPairs = false);

    /// <summary>
    /// Retrieves a word set by ID for a user, including word pairs.
    /// </summary>
    /// <param name="wordSetId">The ID of the word set.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The word set with its word pairs.</returns>
    WordSet GetWordSetById(int wordSetId, string userId);
}