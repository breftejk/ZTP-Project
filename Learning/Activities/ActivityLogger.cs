using ZTP_Project.Models;
using ZTP_Project.Data.Repositories;

namespace ZTP_Project.Learning.Activities
{
    /// <summary>
    /// An activity logger that observes and logs user activities.
    /// </summary>
    public class ActivityLogger : IActivityObserver
    {
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogger"/> class.
        /// </summary>
        /// <param name="activityLogRepository">The activity log repository.</param>
        public ActivityLogger(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        /// <summary>
        /// Updates the observer with a new activity log entry.
        /// </summary>
        /// <param name="log">The activity log.</param>
        public async Task UpdateAsync(ActivityLog log)
        {
            Console.WriteLine("The activity logger has received a new log.");
            await _activityLogRepository.AddAsync(log);
            await _activityLogRepository.SaveChangesAsync();
        }
    }
}