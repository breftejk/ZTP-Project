using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

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
    public List<WordPair> GetAllWordPairs(string language)
    {
        return _dbContext.WordPairs
            .Where(pair => pair.Language.ToLower() == language.ToLower())
            .ToList();
    }

    /// <inheritdoc />
    public List<string> GetTranslations(string word, string language)
    {
        return _dbContext.WordPairs
            .Where(pair => pair.Word.ToLower() == word.ToLower() &&
                           pair.Language.ToLower() == language.ToLower())
            .Select(pair => pair.Translation)
            .ToList();
    }

    /// <inheritdoc />
    public void AddWordPair(string word, string translation, string language)
    {
        var wordPair = new WordPair(word, translation, language.ToLower());
        _dbContext.WordPairs.Add(wordPair);
        _dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public bool IsLanguageSupported(string language)
    {
        return _dbContext.WordPairs
            .Any(pair => pair.Language.ToLower() == language.ToLower());
    }
}