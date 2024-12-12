using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Learning.Activities;
using ZTP_Project.Models;
using ZTP_Project.Learning.Strategies;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller responsible for managing the learning process, including selecting modes and retrieving questions.
    /// </summary>
    public class LearningController : BaseController
    {
        private readonly ILearningStrategyFactory _factory;
        private readonly IGroupRepository _groupRepository;
        private readonly IWordRepository _wordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningController"/> class.
        /// </summary>
        /// <param name="factory">Strategies factory for learning modes.</param>
        /// <param name="groupRepository">Repository for groups.</param>
        /// <param name="wordRepository">Repository for words.</param>
        /// <param name="notifier">Activity notifier.</param>
        /// <param name="logger">Activity logger observer.</param>
        public LearningController(
            ILearningStrategyFactory factory,
            IGroupRepository groupRepository,
            IWordRepository wordRepository,
            ActivityNotifier notifier,
            IActivityObserver logger
        )
        {
            _factory = factory;
            _groupRepository = groupRepository;
            _wordRepository = wordRepository;
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
        public async Task<IActionResult> SubmitAnswer(int wordId, string userAnswer, string mode)
        {
            var userId = GetUserId();
            var learningMode = (LearningMode)Enum.Parse(typeof(LearningMode), mode, true);
            var strategy = _factory.GetStrategy(learningMode);
            var result = await strategy.EvaluateAnswer(userId, wordId, userAnswer);
            return View("Result", result);
        }

        /// <summary>
        /// Displays a question view. If starting a new session, initializes session variables.
        /// </summary>
        /// <param name="groupId">The selected group ID or -1 for all words.</param>
        /// <param name="isNewSession">Indicates whether this is a new session.</param>
        /// <returns>The question view.</returns>
        public async Task<IActionResult> Question(int? groupId = null, bool isNewSession = false)
        {
            var mode = HttpContext.Session.GetString("CurrentMode");

            if (isNewSession)
            {
                if (groupId == null)
                {
                    TempData["Error"] = "You must choose a group or 'All' to start the learning.";
                    return RedirectToAction("SelectMode");
                }

                mode = HttpContext.Request.Query["mode"];
                if (string.IsNullOrEmpty(mode))
                {
                    TempData["Error"] = "You must choose a learning mode.";
                    return RedirectToAction("SelectMode");
                }

                HttpContext.Session.SetInt32("CurrentGroupId", groupId.Value);
                HttpContext.Session.SetString("CurrentMode", mode);
            }

            var currentGroupId = HttpContext.Session.GetInt32("CurrentGroupId");
            if (currentGroupId == null || string.IsNullOrEmpty(mode))
            {
                TempData["Error"] = "No active session.";
                return RedirectToAction("SelectMode");
            }

            var languageId = GetSelectedLanguage().Value;
            IEnumerable<Word> words;

            if (currentGroupId == -1)
            {
                words = await _wordRepository.GetAllAsync();
                if (!words.Any())
                {
                    TempData["Error"] = "No words found for the selected language.";
                    return RedirectToAction("SelectMode");
                }

                words = words.Where(w => w.LanguageId == languageId).ToArray();
            }
            else
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(currentGroupId.Value);
                if (group == null || !group.GroupWords.Any())
                {
                    TempData["Error"] = "No words found in the selected group.";
                    return RedirectToAction("SelectMode");
                }

                words = group.GroupWords.Select(gw => gw.Word);
            }

            var random = new Random();
            var selectedWord = words.ElementAt(random.Next(words.Count()));

            ViewBag.Mode = mode;

            if (mode == "MultipleChoice")
            {
                var allWords = await _wordRepository.GetAllAsync();
                var translations = allWords.Select(w => w.Translation).Distinct().ToList();
                translations.Remove(selectedWord.Translation);
                var wrongOptions = translations.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                wrongOptions.Add(selectedWord.Translation);
                var options = wrongOptions.OrderBy(x => Guid.NewGuid()).ToList();
                ViewBag.Options = options;
            }

            return View("Question", selectedWord);
        }
    }
}