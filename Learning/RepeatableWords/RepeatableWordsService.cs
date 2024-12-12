using ZTP_Project.Data.Repositories;
using ZTP_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZTP_Project.Learning.RepeatableWords
{
    /// <summary>
    /// Service responsible for fetching words for repetition.
    /// </summary>
    public class RepeatableWordsService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IWordRepository _wordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatableWordsService"/> class.
        /// </summary>
        /// <param name="activityLogRepository">The activity log repository.</param>
        /// <param name="wordRepository">The word repository.</param>
        public RepeatableWordsService(IActivityLogRepository activityLogRepository, IWordRepository wordRepository)
        {
            _activityLogRepository = activityLogRepository;
            _wordRepository = wordRepository;
        }

        /// <summary>
        /// Retrieves a list of words that the user needs to repeat based on recent activity logs.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="languageId">The identifier of the language.</param>
        /// <returns>A list of words to repeat.</returns>
        public async Task<List<Word>> GetWordsToRepeatAsync(string userId, int languageId)
        {
            // Retrieve logs for the last 7 days
            var recentLogs = await _activityLogRepository.GetRecentLogsAsync(userId, languageId, 7);

            // Filter only incorrect and uncorrected words
            var incorrectWordIds = recentLogs
                .Where(log => !log.IsCorrect && !log.Corrected)
                .Select(log => log.WordId)
                .Distinct();

            // Fetch words for the selected language that need repetition
            var wordsToRepeat = await _wordRepository.FindAsync(word =>
                incorrectWordIds.Contains(word.Id) && word.LanguageId == languageId);

            return wordsToRepeat.ToList();
        }
    }
}