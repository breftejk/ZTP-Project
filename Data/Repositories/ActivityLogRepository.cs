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

        /// <summary>
        /// Retrieves all activity logs associated with a specific user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>An enumerable collection of activity logs.</returns>
        public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(log => log.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Retrieves recent activity logs for a specific user, filtered by a number of days.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="days">The number of days to look back from the current date.</param>
        /// <returns>An enumerable collection of recent activity logs.</returns>
        public async Task<IEnumerable<ActivityLog>> GetRecentLogsAsync(string userId, int days)
        {
            var cutoff = DateTime.Now.AddDays(-days);
            return await _dbSet.Where(log => log.UserId == userId && log.Timestamp >= cutoff).ToListAsync();
        }
    }
}