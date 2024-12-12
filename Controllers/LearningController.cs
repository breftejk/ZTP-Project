using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Learning.Activities;
using ZTP_Project.Models;
using ZTP_Project.Learning.Strategies;
using ZTP_Project.Data.Repositories;
using ZTP_Project.Learning.RepeatableWords;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller responsible for managing the learning process, including selecting modes and retrieving questions.
    /// </summary>
    public class LearningController : BaseController
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ILearningStrategies _learningStrategies;
        private readonly IGroupRepository _groupRepository;
        private readonly IWordRepository _wordRepository;
        private readonly RepeatableWordsService _repeatableWordsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningController"/> class.
        /// </summary>
        /// <param name="learningStrategies">Factory for learning strategies.</param>
        /// <param name="groupRepository">Repository for group operations.</param>
        /// <param name="wordRepository">Repository for word operations.</param>
        /// <param name="repeatableWordsService">Service for managing repeatable words.</param>
        /// <param name="notifier">Notifier for activity logging.</param>
        /// <param name="logger">Observer for activity notifications.</param>
        /// <param name="activityLogRepository">Repository for activity logs.</param>
        public LearningController(
            ILearningStrategies learningStrategies,
            IGroupRepository groupRepository,
            IWordRepository wordRepository,
            RepeatableWordsService repeatableWordsService,
            ActivityNotifier notifier,
            IActivityObserver logger,
            IActivityLogRepository activityLogRepository
        )
        {
            _activityLogRepository = activityLogRepository;
            _learningStrategies = learningStrategies;
            _groupRepository = groupRepository;
            _wordRepository = wordRepository;
            _repeatableWordsService = repeatableWordsService;
            notifier.Attach(logger);
        }

        /// <summary>
        /// Displays the mode selection view.
        /// </summary>
        /// <returns>The mode selection view.</returns>
        [LanguageSelected]
        public async Task<IActionResult> SelectMode()
        {
            var userId = GetUserId();
            var languageId = GetSelectedLanguage().Value;
            var groups = await _groupRepository.GetGroupsWithLanguageAsync(userId, languageId);
            ViewBag.IncludeAllOption = true;
            ViewBag.IncludeRepeatOption = true;
            return View(groups);
        }

        /// <summary>
        /// Submits the user's answer and evaluates it.
        /// </summary>
        /// <param name="wordId">The ID of the word.</param>
        /// <param name="userAnswer">The user's answer.</param>
        /// <param name="mode">The learning mode.</param>
        /// <returns>The result view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAnswer(int wordId, string userAnswer, string mode)
        {
            var userId = GetUserId();
            var learningMode = Enum.Parse<LearningMode>(mode, true);
            var strategy = _learningStrategies.GetStrategy(learningMode);
            var result = await strategy.EvaluateAnswer(userId, wordId, userAnswer);

            if (result.IsCorrect && HttpContext.Session.GetString("IsRepeatSession") == bool.TrueString)
            {
                // Mark the word as corrected in the activity log
                await _activityLogRepository.MarkAsCorrectedAsync(userId, wordId);
            }

            return View("Result", result);
        }

        /// <summary>
        /// Displays a question view. If starting a new session, initializes session variables.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="isNewSession">Indicates whether this is a new session.</param>
        /// <param name="isRepeatSession">Indicates whether this is a repeat session.</param>
        /// <returns>The question view.</returns>
        public async Task<IActionResult> Question(int? groupId = null, bool isNewSession = false, bool isRepeatSession = false)
        {
            var mode = HttpContext.Session.GetString("CurrentMode");

            if (isNewSession)
            {
                if (groupId == null && !isRepeatSession)
                {
                    TempData["Error"] = "You must choose a group, 'All', or 'Repeatable' to start learning.";
                    return RedirectToAction("SelectMode");
                }

                mode = HttpContext.Request.Query["mode"];
                if (string.IsNullOrEmpty(mode))
                {
                    TempData["Error"] = "You must choose a learning mode.";
                    return RedirectToAction("SelectMode");
                }

                HttpContext.Session.SetInt32("CurrentGroupId", groupId ?? -1);
                HttpContext.Session.SetString("CurrentMode", mode);
                HttpContext.Session.SetString("IsRepeatSession", isRepeatSession.ToString());
            }

            var currentGroupId = HttpContext.Session.GetInt32("CurrentGroupId");
            var isRepeat = HttpContext.Session.GetString("IsRepeatSession") == bool.TrueString;

            if (currentGroupId == null || string.IsNullOrEmpty(mode))
            {
                TempData["Error"] = "No active session.";
                return RedirectToAction("SelectMode");
            }

            var languageId = GetSelectedLanguage().Value;
            IEnumerable<Word> words;

            if (isRepeat)
            {
                words = await _repeatableWordsService.GetWordsToRepeatAsync(GetUserId(), languageId);
                if (!words.Any())
                {
                    TempData["Error"] = "No repeatable words available.";
                    return RedirectToAction("SelectMode");
                }
            }
            else if (currentGroupId == -1)
            {
                words = await _wordRepository.GetAllAsync();
                words = words.Where(w => w.LanguageId == languageId).ToArray();
            }
            else
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(currentGroupId.Value);
                words = group.GroupWords.Select(gw => gw.Word);
            }

            var random = new Random();
            var selectedWord = words.ElementAt(random.Next(words.Count()));

            ViewBag.Mode = mode;
            return View("Question", selectedWord);
        }
    }
}