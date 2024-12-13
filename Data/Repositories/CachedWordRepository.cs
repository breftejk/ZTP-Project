using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Proxy for IWordRepository that adds caching functionality.
    /// </summary>
    public class CachedWordRepositoryProxy : IWordRepository
    {
        private readonly IWordRepository _innerRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedWordRepositoryProxy> _logger;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedWordRepositoryProxy"/> class.
        /// </summary>
        /// <param name="innerRepository">The actual word repository.</param>
        /// <param name="cache">The memory cache instance.</param>
        /// <param name="logger">The logger instance.</param>
        public CachedWordRepositoryProxy(IWordRepository innerRepository, IMemoryCache cache, ILogger<CachedWordRepositoryProxy> logger)
        {
            _innerRepository = innerRepository;
            _cache = cache;
            _logger = logger;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        /// <inheritdoc />
        public async Task AddAsync(Word entity)
        {
            await _innerRepository.AddAsync(entity);
            _cache.Remove($"Word_{entity.Id}");
        }

        /// <inheritdoc />
        public async Task AddRangeAsync(IEnumerable<Word> entities)
        {
            await _innerRepository.AddRangeAsync(entities);
            foreach (var entity in entities)
            {
                _cache.Remove($"Word_{entity.Id}");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Word>> FindAsync(Expression<Func<Word, bool>> predicate, params Expression<Func<Word, object>>[] includes)
        {
            return await _innerRepository.FindAsync(predicate, includes);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Word>> GetAllAsync()
        {
            string cacheKey = "AllWords";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<Word> cachedWords))
            {
                _logger.LogInformation("Cache hit for GetAllAsync.");
                return cachedWords;
            }

            _logger.LogInformation("Cache miss for GetAllAsync. Fetching from repository.");
            var words = await _innerRepository.GetAllAsync();
            _cache.Set(cacheKey, words, _cacheOptions);
            return words;
        }

        /// <inheritdoc />
        public async Task<Word?> GetByIdAsync(int id, params Expression<Func<Word, object>>[] includes)
        {
            string cacheKey = $"Word_{id}";

            if (_cache.TryGetValue(cacheKey, out Word cachedWord))
            {
                _logger.LogInformation($"Cache hit for GetByIdAsync with ID: {id}");
                return cachedWord;
            }

            _logger.LogInformation($"Cache miss for GetByIdAsync with ID: {id}. Fetching from repository.");
            var word = await _innerRepository.GetByIdAsync(id, includes);

            if (word != null)
            {
                _cache.Set(cacheKey, word, _cacheOptions);
                _logger.LogInformation($"Word ID: {id} cached.");
            }

            return word;
        }

        /// <inheritdoc />
        public void Remove(Word entity)
        {
            _innerRepository.Remove(entity);
            _cache.Remove($"Word_{entity.Id}");
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            await _innerRepository.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Word> Words, int TotalCount)> GetPaginatedAsync(int page, int pageSize)
        {
            return await _innerRepository.GetPaginatedAsync(page, pageSize);
        }
    }
}