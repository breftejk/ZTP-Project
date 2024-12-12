using ZTP_Project.Models;

namespace ZTP_Project.Learning.Challenges
{
    /// <summary>
    /// Defines a contract for managing daily challenges.
    /// </summary>
    public interface IDailyChallengeService
    {
        /// <summary>
        /// Retrieves or creates a daily challenge for the specified user and language.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>The active daily challenge if it exists or a newly created one.</returns>
        Task<DailyChallenge?> GetOrCreateDailyChallengeAsync(string userId, int languageId);

        /// <summary>
        /// Marks the specified challenge as completed.
        /// </summary>
        /// <param name="challengeId">The unique identifier of the challenge.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CompleteChallengeAsync(int challengeId);
    }
}