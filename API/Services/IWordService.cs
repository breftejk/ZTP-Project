using API.Models;

namespace API.Services;

/// <summary>
/// Interface for managing word pairs.
/// </summary>
public interface IWordService
{
    /// <summary>
    /// Retrieves all word pairs for a specific language code, or all word pairs if no language code is specified.
    /// </summary>
    /// <param name="languageCode">
    /// The language code to filter the word pairs. 
    /// If <c>null</c>, all word pairs from all languages will be returned.
    /// </param>
    /// <returns>
    /// A list of all word pairs in the specified language, 
    /// or all word pairs if <paramref name="languageCode"/> is <c>null</c>.
    /// </returns>
    List<WordPair> GetAllWordPairs(string? languageCode);

    /// <summary>
    /// Retrieves translations for a specific word in a specific language code.
    /// </summary>
    /// <param name="word">The word to translate.</param>
    /// <param name="languageCode">The language code to filter the translations.</param>
    /// <returns>A list of translations for the specified word in the specified language code.</returns>
    List<string> GetTranslations(string word, string languageCode);

    /// <summary>
    /// Adds a new word pair to the database.
    /// </summary>
    /// <param name="word">The source word.</param>
    /// <param name="translation">The translation of the word.</param>
    /// <param name="languageCode">The language code of the word pair.</param>
    WordPair AddWordPair(string word, string translation, string languageCode);

    /// <summary>
    /// Checks if a language code is supported by verifying if at least one word pair exists.
    /// </summary>
    /// <param name="languageCode">The language code to check for support.</param>
    /// <returns>True if the language code is supported, otherwise false.</returns>
    bool IsLanguageSupported(string languageCode);
}