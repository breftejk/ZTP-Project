using ZTP_Project.Models;
using ZTP_Project.Learning.Activities;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Learning.Strategies
{
    /// <summary>
    /// Represents the "Flashcards" learning strategy.
    /// </summary>
    public class FlashcardsStrategy : ILearningStrategy
    {
        private readonly ActivityNotifier _notifier;
        private readonly IWordRepository _wordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlashcardsStrategy"/> class.
        /// </summary>
        /// <param name="notifier">The activity notifier.</param>
        /// <param name="wordRepository">The word repository.</param>
        public FlashcardsStrategy(ActivityNotifier notifier, IWordRepository wordRepository)
        {
            _notifier = notifier;
            _wordRepository = wordRepository;
        }

        /// <inheritdoc/>
        public async Task<LearningResult> EvaluateAnswer(string userId, int wordId, string userAnswer)
        {
            var word = await _wordRepository.GetByIdAsync(wordId);
            if (word == null)
                throw new Exception("Word not found.");

            var correctAnswer = word.Translation;
            var isCorrect = string.Equals(userAnswer, correctAnswer, StringComparison.OrdinalIgnoreCase);

            var log = new ActivityLog
            {
                UserId = userId,
                WordId = wordId,
                IsCorrect = isCorrect,
                Timestamp = DateTime.Now
            };

            await _notifier.NotifyAsync(log);

            return new LearningResult
            {
                IsCorrect = isCorrect,
                CorrectAnswer = correctAnswer,
                UserAnswer = userAnswer
            };
        }
    }
}