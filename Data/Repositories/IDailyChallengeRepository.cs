using System.Threading.Tasks;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Interface defining methods for managing daily challenges.
    /// </summary>
    public interface IDailyChallengeRepository : IRepository<DailyChallenge>
    {
        /// <summary>
        /// Retrieves the active challenge for the current day.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="languageId">Language identifier.</param>
        /// <returns>Active daily challenge if it exists, otherwise null.</returns>
        Task<DailyChallenge?> GetActiveChallengeAsync(string userId, int languageId);

        /// <summary>
        /// Marks a challenge as completed.
        /// </summary>
        /// <param name="challengeId">Challenge identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MarkAsCompletedAsync(int challengeId);
    }
}