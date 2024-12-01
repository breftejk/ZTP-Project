using API.Models;

namespace API.Services;

/// <summary>
/// Interface for managing word pairs.
/// </summary>
public interface IWordService
{
    /// <summary>
    /// Retrieves all word pairs for a specific language, or all word pairs if no language is specified.
    /// </summary>
    /// <param name="language">
    /// The language to filter the word pairs. 
    /// If <c>null</c>, all word pairs from all languages will be returned.
    /// </param>
    /// <returns>
    /// A list of all word pairs in the specified language, 
    /// or all word pairs if <paramref name="language"/> is <c>null</c>.
    /// </returns>
    List<WordPair> GetAllWordPairs(string? language);

    /// <summary>
    /// Retrieves translations for a specific word in a specific language.
    /// </summary>
    /// <param name="word">The word to translate.</param>
    /// <param name="language">The language to filter the translations.</param>
    /// <returns>A list of translations for the specified word in the specified language.</returns>
    List<string> GetTranslations(string word, string language);

    /// <summary>
    /// Adds a new word pair to the database.
    /// </summary>
    /// <param name="word">The source word.</param>
    /// <param name="translation">The translation of the word.</param>
    /// <param name="language">The language of the word pair.</param>
    void AddWordPair(string word, string translation, string language);

    /// <summary>
    /// Checks if a language is supported by verifying if at least one word pair exists.
    /// </summary>
    /// <param name="language">The language to check for support.</param>
    /// <returns>True if the language is supported, otherwise false.</returns>
    bool IsLanguageSupported(string language);
}