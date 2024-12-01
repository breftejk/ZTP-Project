using API.Data;

namespace API.Services;

/// <summary>
/// Factory to create or retrieve instances of <see cref="IWordService"/>.
/// </summary>
public class WordServiceFactory
{
    private readonly AppDbContext _dbContext;
    private readonly Dictionary<string, IWordService> _services = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="WordServiceFactory"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used by the services.</param>
    public WordServiceFactory(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Checks if a specific language code is supported by verifying if there are any word pairs in the database.
    /// </summary>
    /// <param name="languageCode">The language code to check for support.</param>
    /// <returns>True if the language code is supported, otherwise false.</returns>
    public bool IsLanguageSupported(string languageCode)
    {
        var service = GetService();
        return service.IsLanguageSupported(languageCode);
    }

    /// <summary>
    /// Retrieves or creates a default instance of <see cref="IWordService"/>.
    /// </summary>
    /// <returns>An instance of <see cref="IWordService"/>.</returns>
    public IWordService GetService()
    {
        const string key = "default";

        if (!_services.ContainsKey(key))
        {
            _services[key] = new WordService(_dbContext);
        }

        return _services[key];
    }
}