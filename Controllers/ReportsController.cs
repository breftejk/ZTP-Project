using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Attributes;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller responsible for displaying user activity reports.
    /// </summary>
    [Authorize]
    [LanguageSelected]
    public class ReportsController : BaseController
    {
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="activityLogRepository">The activity log repository.</param>
        public ReportsController(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        /// <summary>
        /// Displays the user activity report for the specified number of days.
        /// </summary>
        /// <param name="days">The number of days to consider.</param>
        /// <returns>The user activity view.</returns>
        public async Task<IActionResult> UserActivity(int days = 7)
        {
            var userId = GetUserId();
            var languageId = GetSelectedLanguage().Value;

            var logs = await _activityLogRepository.GetRecentLogsAsync(userId, languageId, days);
            var correctCount = logs.Count(l => l.IsCorrect);
            var wrongCount = logs.Count(l => !l.IsCorrect);
            var correctedCount = logs.Count(l => !l.IsCorrect && !l.Corrected);

            ViewBag.CorrectCount = correctCount;
            ViewBag.WrongCount = wrongCount;
            ViewBag.CorrectedCount = correctedCount;
            ViewBag.Days = days;

            return View("UserActivity");
        }
    }
}