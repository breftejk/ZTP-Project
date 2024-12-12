using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing activity logs in the database.
    /// </summary>
    public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ActivityLogRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(log => log.UserId == userId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task MarkAsCorrectedAsync(string userId, int wordId)
        {
            var log = await _dbSet
                .FirstOrDefaultAsync(l => l.UserId == userId && l.WordId == wordId && !l.Corrected);

            if (log != null)
            {
                log.Corrected = true;
                await SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ActivityLog>> GetRecentLogsAsync(string userId, int languageId, int days)
        {
            var cutoff = DateTime.Now.AddDays(-days);

            return await _dbSet
                .Where(log => log.UserId == userId && log.Timestamp >= cutoff)
                .Join(
                    _context.Words, // Join with Words
                    log => log.WordId, // Match WordId in ActivityLog
                    word => word.Id, // Match Id in Word
                    (log, word) => new { log, word } // Create a new result
                )
                .Where(joined => joined.word.LanguageId == languageId) // Filter by LanguageId
                .Select(joined => joined.log) // Select the ActivityLog
                .ToListAsync();
        }
    }
}