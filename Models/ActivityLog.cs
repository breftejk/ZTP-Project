namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents an activity log entry for user learning sessions.
    /// </summary>
    public class ActivityLog
    {
        /// <summary>
        /// Gets or sets the unique identifier of the activity log.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with this activity.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the word involved in the activity.
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's answer was correct.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the word was corrected after an incorrect answer.
        /// </summary>
        public bool Corrected { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the activity occurred.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}