using System.Collections.Generic;
using System.Threading.Tasks;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Interface for word-specific repository operations.
    /// </summary>
    public interface IWordRepository : IRepository<Word>
    {
        /// <summary>
        /// Retrieves a paginated list of words along with the total count.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A tuple containing the list of words and the total count.</returns>
        Task<(IEnumerable<Word> Words, int TotalCount)> GetPaginatedAsync(int page, int pageSize);

        /// <summary>
        /// Gets random words excluding the specified word ID.
        /// </summary>
        /// <param name="excludedWordId">The ID of the word to exclude.</param>
        /// <param name="count">The number of random words to retrieve.</param>
        /// <returns>A list of random words.</returns>
        Task<List<Word>> GetRandomWordsAsync(int excludedWordId, int count);
    }
}