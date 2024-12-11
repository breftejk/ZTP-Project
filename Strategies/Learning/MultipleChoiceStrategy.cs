using ZTP_Project.Models;
using ZTP_Project.Observers;
using ZTP_Project.Repositories;

namespace ZTP_Project.Strategies.Learning
{
    /// <summary>
    /// Represents the "Multiple Choice" learning strategy.
    /// </summary>
    public class MultipleChoiceStrategy : ILearningStrategy
    {
        private readonly ActivityNotifier _notifier;
        private readonly IWordRepository _wordRepository;
        private readonly Random _random = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceStrategy"/> class.
        /// </summary>
        /// <param name="notifier">The activity notifier.</param>
        /// <param name="wordRepository">The word repository.</param>
        public MultipleChoiceStrategy(ActivityNotifier notifier, IWordRepository wordRepository)
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