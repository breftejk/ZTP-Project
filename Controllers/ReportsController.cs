using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ZTP_Project.Models;
using ZTP_Project.Repositories;

namespace ZTP_Project.Controllers
{
    /// <summary>
    /// Controller responsible for displaying user activity reports.
    /// </summary>
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
            var logs = await _activityLogRepository.GetRecentLogsAsync(userId, days);
            var correctCount = logs.Count(l => l.IsCorrect);
            var wrongCount = logs.Count(l => !l.IsCorrect);

            ViewBag.CorrectCount = correctCount;
            ViewBag.WrongCount = wrongCount;
            ViewBag.Days = days;

            return View("UserActivity");
        }
    }
}