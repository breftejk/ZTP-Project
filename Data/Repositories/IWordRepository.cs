using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Interface for word-specific repository operations.
    /// </summary>
    public interface IWordRepository : IRepository<Word>
    {
        Task<(IEnumerable<Word> Words, int TotalCount)> GetPaginatedAsync(int page, int pageSize);
    }
}