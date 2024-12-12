using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing words.
    /// </summary>
    public class WordRepository : Repository<Word>, IWordRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public WordRepository(ApplicationDbContext context) : base(context) { }
        
        /// <inheritdoc />
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