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
    /// Retrieves all word pairs for a specified language, or all word pairs if no language is specified.
    /// </summary>
    /// <param name="language">
    /// The language for which to retrieve word pairs. 
    /// If <c>null</c>, all word pairs from all languages will be returned.
    /// </param>
    /// <returns>
    /// A list of <see cref="WordPair"/> objects. 
    /// Returns all word pairs if <paramref name="language"/> is <c>null</c>.
    /// </returns>
    /// <exception cref="NotSupportedException">Thrown if the specified language is not supported.</exception>
    public List<WordPair> GetAllWords(string? language)
    {
        var service = _factory.GetService();
        
        if (language is null)
        {
            return service.GetAllWordPairs(null);
        }
        
        if (language is not null && !_factory.IsLanguageSupported(language))
        {
            throw new NotSupportedException($"Language '{language}' is not supported.");
        }

        return service.GetAllWordPairs(language);
    }

    /// <summary>
    /// Retrieves translations for a specific word in a specified language.
    /// </summary>
    /// <param name="word">The word to translate.</param>
    /// <param name="language">The language to filter translations.</param>
    /// <returns>A list of translations for the word.</returns>
    /// <exception cref="NotSupportedException">Thrown if the language is not supported.</exception>
    public List<string> GetTranslations(string word, string language)
    {
        if (!_factory.IsLanguageSupported(language))
        {
            throw new NotSupportedException($"Language '{language}' is not supported.");
        }

        var service = _factory.GetService();
        return service.GetTranslations(word, language);
    }

    /// <summary>
    /// Adds a new word pair to the database.
    /// </summary>
    /// <param name="word">The source word.</param>
    /// <param name="translation">The translation of the word.</param>
    /// <param name="language">The language of the word pair.</param>
    public void AddWordPair(string word, string translation, string language)
    {
        var service = _factory.GetService();
        service.AddWordPair(word, translation, language);
    }
}