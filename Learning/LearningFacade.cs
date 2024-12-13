using ZTP_Project.Data.Repositories;
using ZTP_Project.Learning.Strategies;
using ZTP_Project.Learning.RepeatableWords;
using ZTP_Project.Models;

namespace ZTP_Project.Learning
{
    /// <summary>
    /// Facade class responsible for managing the learning process, providing a simplified interface for various learning operations.
    /// </summary>
    public class LearningFacade : ILearningFacade
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IWordRepository _wordRepository;
        private readonly ILearningStrategies _learningStrategies;
        private readonly RepeatableWordsService _repeatableWordsService;
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningFacade"/> class.
        /// </summary>
        /// <param name="groupRepository">Repository for managing groups.</param>
        /// <param name="wordRepository">Repository for managing words.</param>
        /// <param name="learningStrategies">Strategies for different learning modes.</param>
        /// <param name="repeatableWordsService">Service for managing words that need to be repeated.</param>
        /// <param name="activityLogRepository">Repository for logging user activities.</param>
        public LearningFacade(
            IGroupRepository groupRepository,
            IWordRepository wordRepository,
            ILearningStrategies learningStrategies,
            RepeatableWordsService repeatableWordsService,
            IActivityLogRepository activityLogRepository)
        {
            _groupRepository = groupRepository;
            _wordRepository = wordRepository;
            _learningStrategies = learningStrategies;
            _repeatableWordsService = repeatableWordsService;
            _activityLogRepository = activityLogRepository;
        }

        /// <summary>
        /// Retrieves groups associated with the specified user and language.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="languageId">The language ID.</param>
        /// <returns>A list of groups associated with the user and language.</returns>
        public async Task<IEnumerable<Group>> GetGroupsWithLanguageAsync(string userId, int languageId)
        {
            return await _groupRepository.GetGroupsWithLanguageAsync(userId, languageId);
        }

        /// <summary>
        /// Evaluates the user's answer to a specific word based on the selected learning mode.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="wordId">The word's ID.</param>
        /// <param name="userAnswer">The user's answer.</param>
        /// <param name="mode">The learning mode.</param>
        /// <returns>The result of the evaluation.</returns>
        public async Task<LearningResult> EvaluateAnswerAsync(string userId, int wordId, string userAnswer, LearningMode mode)
        {
            var strategy = _learningStrategies.GetStrategy(mode);
            var result = await strategy.EvaluateAnswer(userId, wordId, userAnswer);

            if (result.IsCorrect)
            {
                await _activityLogRepository.MarkAsCorrectedAsync(userId, wordId);
            }

            return result;
        }

        /// <summary>
        /// Retrieves words that the user needs to repeat based on their language preferences.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="languageId">The language ID.</param>
        /// <returns>A list of words that need to be repeated.</returns>
        public async Task<IEnumerable<Word>> GetWordsToRepeatAsync(string userId, int languageId)
        {
            return await _repeatableWordsService.GetWordsToRepeatAsync(userId, languageId);
        }

        /// <summary>
        /// Retrieves all words for the specified language.
        /// </summary>
        /// <param name="languageId">The language ID.</param>
        /// <returns>A list of all words associated with the specified language.</returns>
        public async Task<IEnumerable<Word>> GetAllWordsAsync(int languageId)
        {
            var words = await _wordRepository.GetAllAsync();
            return words.Where(w => w.LanguageId == languageId);
        }

        /// <summary>
        /// Retrieves words from a specific group.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>A list of words from the specified group.</returns>
        public async Task<IEnumerable<Word>> GetGroupWordsAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
            return group.GroupWords.Select(gw => gw.Word);
        }

        /// <summary>
        /// Generates a set of multiple-choice options for a word, including the correct answer and random incorrect options.
        /// </summary>
        /// <param name="wordId">The word's ID.</param>
        /// <returns>A shuffled list of options including the correct answer.</returns>
        public async Task<List<string>> GetMultipleChoiceOptionsAsync(int wordId)
        {
            var correctWord = await _wordRepository.GetByIdAsync(wordId);
            if (correctWord == null)
            {
                throw new ArgumentException("Word not found", nameof(wordId));
            }

            var incorrectWords = await _wordRepository.GetRandomWordsAsync(wordId, 3);

            var options = new List<string> { correctWord.Translation };
            options.AddRange(incorrectWords.Select(w => w.Translation));

            return options.OrderBy(o => Guid.NewGuid()).ToList();
        }
    }
}