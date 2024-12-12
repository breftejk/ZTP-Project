using System.Collections.Generic;
using System.Threading.Tasks;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Interface defining methods for an activity log repository.
    /// </summary>
    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
        /// <summary>
        /// Retrieves all activity logs for a specified user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>An enumerable collection of activity logs.</returns>
        Task<IEnumerable<ActivityLog>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Retrieves activity logs for a specified user within a given time frame.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="languageId">The language ID.</param>
        /// <param name="days">The number of days to look back.</param>
        /// <returns>An enumerable collection of recent activity logs.</returns>
        Task<IEnumerable<ActivityLog>> GetRecentLogsAsync(string userId, int languageId, int days);

        /// <summary>
        /// Marks a word as corrected in the activity log.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="wordId">The word's ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MarkAsCorrectedAsync(string userId, int wordId);
    }
}