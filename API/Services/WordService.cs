using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Word;

/// <summary>
/// Service for managing word pairs stored in the database.
/// </summary>
public class WordService : IWordService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used by the service.</param>
    public WordService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public List<WordPair> GetAllWordPairs(string? languageCode)
    {
        return _dbContext.WordPairs
            .Where(wp => string.IsNullOrEmpty(languageCode) || wp.LanguageCode.ToLower() == languageCode.ToLower())
            .ToList();
    }

    /// <inheritdoc />
    public List<string> GetTranslations(string word, string languageCode)
    {
        return _dbContext.WordPairs
            .Where(wp => wp.Word.ToLower() == word.ToLower() &&
                         wp.LanguageCode.ToLower() == languageCode.ToLower())
            .Select(wp => wp.Translation)
            .ToList();
    }

    /// <inheritdoc />
    public WordPair AddWordPair(string word, string translation, string languageCode)
    {
        var wordPair = new WordPair
        {
            Word = word,
            Translation = translation,
            LanguageCode = languageCode.ToLower()
        };

        _dbContext.WordPairs.Add(wordPair);
        _dbContext.SaveChanges();

        return wordPair;
    }

    /// <inheritdoc />
    public bool IsLanguageSupported(string languageCode)
    {
        return _dbContext.Languages.Any(l => l.Code.ToLower() == languageCode.ToLower());
    }
    
    public bool WordLanguagePresent(string word, string languageCode)
    {
        return _dbContext.WordPairs.Any(wp => wp.Word.ToLower() == word.ToLower() && wp.LanguageCode.ToLower() == languageCode.ToLower());
    }
}
