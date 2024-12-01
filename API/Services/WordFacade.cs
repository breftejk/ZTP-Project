using API.Models;

namespace API.Services;

/// <summary>
/// Facade for simplifying interactions with word-related operations.
/// </summary>
public class WordFacade
{
    private readonly WordServiceFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordFacade"/> class.
    /// </summary>
    /// <param name="factory">The factory used to create or retrieve word services.</param>
    public WordFacade(WordServiceFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Retrieves all word pairs for a specified language code, or all word pairs if no language code is specified.
    /// </summary>
    /// <param name="languageCode">
    /// The language code for which to retrieve word pairs. 
    /// If <c>null</c>, all word pairs from all languages will be returned.
    /// </param>
    /// <returns>
    /// A list of <see cref="WordPair"/> objects. 
    /// Returns all word pairs if <paramref name="languageCode"/> is <c>null</c>.
    /// </returns>
    /// <exception cref="NotSupportedException">Thrown if the specified language code is not supported.</exception>
    public List<WordPair> GetAllWords(string? languageCode)
    {
        var service = _factory.GetService();

        if (languageCode is null)
        {
            return service.GetAllWordPairs(null);
        }

        if (!_factory.IsLanguageSupported(languageCode))
        {
            throw new NotSupportedException($"Language code '{languageCode}' is not supported.");
        }

        return service.GetAllWordPairs(languageCode);
    }

    /// <summary>
    /// Retrieves translations for a specific word in a specified language code.
    /// </summary>
    /// <param name="word">The word to translate.</param>
    /// <param name="languageCode">The language code to filter translations.</param>
    /// <returns>A list of translations for the word.</returns>
    /// <exception cref="NotSupportedException">Thrown if the language code is not supported.</exception>
    public List<string> GetTranslations(string word, string languageCode)
    {
        if (!_factory.IsLanguageSupported(languageCode))
        {
            throw new NotSupportedException($"Language code '{languageCode}' is not supported.");
        }

        var service = _factory.GetService();
        return service.GetTranslations(word, languageCode);
    }

    /// <summary>
    /// Adds a new word pair to the database.
    /// </summary>
    /// <param name="word">The source word.</param>
    /// <param name="translation">The translation of the word.</param>
    /// <param name="languageCode">The language code of the word pair.</param>
    public void AddWordPair(string word, string translation, string languageCode)
    {
        var service = _factory.GetService();
        service.AddWordPair(word, translation, languageCode);
    }
}