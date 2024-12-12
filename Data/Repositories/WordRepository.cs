using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing words.
    /// </summary>
    public class WordRepository : Repository<Word>, IWordRepository
    {
        public WordRepository(ApplicationDbContext context) : base(context) { }
        
        public async Task<(IEnumerable<Word> Words, int TotalCount)> GetPaginatedAsync(int page, int pageSize)
        {
            var totalCount = await _dbSet.CountAsync();
            var words = await _dbSet
                .OrderBy(w => w.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (words, totalCount);
        }
    }
}