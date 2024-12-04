using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Word.Sets;

/// <summary>
/// Service for managing word sets containing word pairs in a specific language for a user.
/// </summary>
public class WordSetService : IWordSetService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordSetService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used by the service.</param>
    public WordSetService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public WordSet CreateWordSet(string name, string languageCode, string userId)
    {
        var wordSet = new WordSet
        {
            Name = name,
            LanguageCode = languageCode.ToLower(),
            UserId = userId
        };

        _dbContext.WordSets.Add(wordSet);
        _dbContext.SaveChanges();

        return wordSet;
    }

    /// <inheritdoc />
    public void AddWordPairsToSet(int wordSetId, List<int> wordPairIds, string userId)
    {
        var wordSet = _dbContext.WordSets
            .Include(ws => ws.WordPairs)
            .FirstOrDefault(ws => ws.Id == wordSetId && ws.UserId == userId);

        if (wordSet == null)
            throw new Exception("Word set not found or access denied.");

        var wordPairs = _dbContext.WordPairs
            .Where(wp => wordPairIds.Contains(wp.Id))
            .ToList();

        // Ensure all word pairs have the same language code as the word set
        if (wordPairs.Any(wp => wp.LanguageCode.ToLower() != wordSet.LanguageCode.ToLower()))
            throw new Exception("All word pairs must have the same language code as the word set.");

        foreach (var wordPair in wordPairs)
        {
            if (!wordSet.WordPairs.Any(wp => wp.Id == wordPair.Id))
            {
                wordSet.WordPairs.Add(wordPair);
            }
        }

        _dbContext.SaveChanges();
    }

    /// <inheritdoc />
    public List<WordSet> GetAllWordSets(string userId, bool includeWordPairs = false)
    {
        var query = _dbContext.WordSets
            .Where(ws => ws.UserId == userId);

        if (includeWordPairs)
        {
            query = query.Include(ws => ws.WordPairs);
        }

        return query.ToList();
    }

    /// <inheritdoc />
    public WordSet GetWordSetById(int wordSetId, string userId)
    {
        var wordSet = _dbContext.WordSets
            .Include(ws => ws.WordPairs)
            .FirstOrDefault(ws => ws.Id == wordSetId && ws.UserId == userId);

        if (wordSet == null)
            throw new Exception("Word set not found or access denied.");

        return wordSet;
    }
}