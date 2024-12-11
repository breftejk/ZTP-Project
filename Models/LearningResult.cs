namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents the result of evaluating a user's answer in a learning session.
    /// </summary>
    public class LearningResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user's answer was correct.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Gets or sets the correct answer for the word.
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets the answer provided by the user.
        /// </summary>
        public string UserAnswer { get; set; }
    }
}