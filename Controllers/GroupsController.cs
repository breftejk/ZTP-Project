using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Models;
using ZTP_Project.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller for managing groups of words for a specific user and language.
    /// </summary>
    [Authorize]
    public class GroupsController : BaseController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IWordRepository _wordRepository;
        private readonly ILanguageRepository _languageRepository;

        public GroupsController(
            IGroupRepository groupRepository,
            IWordRepository wordRepository,
            ILanguageRepository languageRepository)
        {
            _groupRepository = groupRepository;
            _wordRepository = wordRepository;
            _languageRepository = languageRepository;
        }

        /// <summary>
        /// Displays the list of groups for the current user and selected language.
        /// </summary>
        /// <returns>A view with the list of groups.</returns>
        [LanguageSelected]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var languageId = HttpContext.Session.GetInt32("SelectedLanguage");
            var groups = await _groupRepository.GetGroupsWithLanguageAsync(userId, languageId.Value);
            var language = await _languageRepository.GetByIdAsync(languageId.Value);
            ViewBag.CurrentLanguage = language?.Name ?? "Unknown";
            return View(groups);
        }

        /// <summary>
        /// Displays the form for creating a new group.
        /// </summary>
        /// <returns>A view with the group creation form.</returns>
        [HttpGet]
        [LanguageSelected]
        public async Task<IActionResult> CreateGroup()
        {
            var languageId = GetSelectedLanguage();
            var language = await _languageRepository.GetByIdAsync(languageId.Value);
            if (language == null)
            {
                TempData["Error"] = "Selected language does not exist.";
                return RedirectToAction("SelectLanguage", "LanguageSelection");
            }
            return View();
        }

        /// <summary>
        /// Handles the creation of a new group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns>A redirect to the groups list or an error message if creation fails.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(string name)
        {
            var languageId = GetSelectedLanguage();
            if (languageId == null)
            {
                TempData["Error"] = "Please select a language first.";
                return RedirectToAction("SelectLanguage", "LanguageSelection");
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }

            var language = await _languageRepository.GetByIdAsync(languageId.Value);
            if (language == null)
            {
                TempData["Error"] = "Selected language does not exist.";
                return RedirectToAction("SelectLanguage", "LanguageSelection");
            }

            var group = new Group
            {
                Name = name,
                UserId = userId,
                LanguageId = languageId.Value
            };

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            TempData["Success"] = "Group created successfully.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays a list of words that can be added to a group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <returns>A view with the list of words.</returns>
        [HttpGet]
        public async Task<IActionResult> AddWords(int groupId)
        {
            var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
            if (group == null || group.UserId != GetUserId())
            {
                return Forbid();
            }

            var languageId = group.LanguageId;
            var words = await _wordRepository.FindAsync(w => w.LanguageId == languageId);
            var groupWordIds = group.GroupWords.Select(gw => gw.WordId).ToList();

            ViewBag.GroupId = groupId;
            ViewBag.GroupWords = groupWordIds;
            ViewBag.GroupName = group.Name;

            return View(words);
        }

        /// <summary>
        /// Adds a word to a group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="wordId">The ID of the word.</param>
        /// <returns>A redirect to the list of words in the group.</returns>
        [HttpPost]
        public async Task<IActionResult> AddWordToGroup(int groupId, int wordId)
        {
            var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
            if (group == null || group.UserId != GetUserId())
            {
                return Forbid();
            }

            var word = await _wordRepository.GetByIdAsync(wordId);
            if (word == null || word.LanguageId != group.LanguageId)
            {
                TempData["Error"] = "Cannot add a word from a different language.";
                return RedirectToAction(nameof(AddWords), new { groupId });
            }

            if (group.GroupWords.Any(gw => gw.WordId == wordId))
            {
                TempData["Error"] = "This word is already in the group.";
                return RedirectToAction(nameof(AddWords), new { groupId });
            }

            group.GroupWords.Add(new GroupWord { GroupId = groupId, WordId = wordId });
            await _groupRepository.SaveChangesAsync();

            TempData["Success"] = "Word added successfully.";
            return RedirectToAction(nameof(AddWords), new { groupId });
        }

        /// <summary>
        /// Removes a word from a group.
        /// </summary>
        /// <param name="groupId">The ID of the group.</param>
        /// <param name="wordId">The ID of the word.</param>
        /// <returns>A redirect to the list of words in the group.</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveWordFromGroup(int groupId, int wordId)
        {
            var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
            if (group == null || group.UserId != GetUserId())
            {
                return Forbid();
            }

            var groupWord = group.GroupWords.FirstOrDefault(gw => gw.WordId == wordId);
            if (groupWord != null)
            {
                group.GroupWords.Remove(groupWord);
                await _groupRepository.SaveChangesAsync();
                TempData["Success"] = "Word removed successfully.";
            }
            else
            {
                TempData["Error"] = "Word not found in the group.";
            }

            return RedirectToAction(nameof(AddWords), new { groupId });
        }
    }
}