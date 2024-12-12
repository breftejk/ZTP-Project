using System;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing daily challenges.
    /// </summary>
    public class DailyChallengeRepository : Repository<DailyChallenge>, IDailyChallengeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyChallengeRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public DailyChallengeRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<DailyChallenge?> GetActiveChallengeAsync(string userId, int languageId)
        {
            var today = DateTime.Today;

            return await _dbSet
                .Include(dc => dc.Words)
                .FirstOrDefaultAsync(dc =>
                    dc.UserId == userId &&
                    dc.LanguageId == languageId &&
                    dc.CreatedAt >= today && dc.CreatedAt < today.AddDays(1));
        }

        /// <inheritdoc />
        public async Task MarkAsCompletedAsync(int challengeId)
        {
            var challenge = await _dbSet.FindAsync(challengeId);
            if (challenge != null)
            {
                challenge.IsCompleted = true;
                await SaveChangesAsync();
            }
        }
    }
}