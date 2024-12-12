using ZTP_Project.Data.Repositories;
using ZTP_Project.Models;

namespace ZTP_Project.Learning.Challenges
{
    /// <summary>
    /// Service responsible for managing daily challenges.
    /// </summary>
    public class DailyChallengeService : IDailyChallengeService
    {
        private readonly IDailyChallengeRepository _dailyChallengeRepository;
        private readonly IWordRepository _wordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyChallengeService"/> class.
        /// </summary>
        /// <param name="dailyChallengeRepository">Repository for daily challenge operations.</param>
        /// <param name="wordRepository">Repository for word operations.</param>
        public DailyChallengeService(IDailyChallengeRepository dailyChallengeRepository, IWordRepository wordRepository)
        {
            _dailyChallengeRepository = dailyChallengeRepository;
            _wordRepository = wordRepository;
        }

        /// <inheritdoc />
        public async Task<DailyChallenge?> GetOrCreateDailyChallengeAsync(string userId, int languageId)
        {
            // Retrieve the active challenge for today
            var activeChallenge = await _dailyChallengeRepository.GetActiveChallengeAsync(userId, languageId);

            if (activeChallenge != null)
            {
                return activeChallenge;
            }

            // Fetch words for the selected language
            var wordsForLanguage = await _wordRepository.FindAsync(word => word.LanguageId == languageId);

            if (!wordsForLanguage.Any())
            {
                // No words available to create a challenge
                return null;
            }

            // Randomly select up to 10 unique words
            var random = new Random();
            var selectedWords = wordsForLanguage
                .OrderBy(_ => random.Next())
                .Take(10)
                .ToList();

            // Create a new daily challenge
            var newChallenge = new DailyChallenge
            {
                UserId = userId,
                LanguageId = languageId,
                Words = selectedWords,
                CreatedAt = DateTime.Today,
                IsCompleted = false
            };

            await _dailyChallengeRepository.AddAsync(newChallenge);
            await _dailyChallengeRepository.SaveChangesAsync();

            return newChallenge;
        }

        /// <inheritdoc />
        public async Task CompleteChallengeAsync(int challengeId)
        {
            await _dailyChallengeRepository.MarkAsCompletedAsync(challengeId);
        }
    }
}