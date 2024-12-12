using System.ComponentModel.DataAnnotations;

namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents a daily challenge for a user.
    /// </summary>
    public class DailyChallenge
    {
        /// <summary>
        /// Gets or sets the unique identifier of the daily challenge.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the challenge.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the language ID for the challenge.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the list of words included in the challenge.
        /// </summary>
        public List<Word> Words { get; set; } = new();

        /// <summary>
        /// Gets or sets the date when the challenge was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the challenge was completed.
        /// </summary>
        public bool IsCompleted { get; set; } = false;
    }
}