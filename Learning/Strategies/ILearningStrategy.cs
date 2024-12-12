using ZTP_Project.Models;

namespace ZTP_Project.Learning.Strategies
{
    /// <summary>
    /// Defines the interface for a learning strategy that evaluates a user's answer.
    /// </summary>
    public interface ILearningStrategy
    {
        /// <summary>
        /// Evaluates the user's answer for a given word.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="wordId">The word ID.</param>
        /// <param name="userAnswer">The user's answer.</param>
        /// <returns>The learning result containing correctness and correct answer details.</returns>
        Task<LearningResult> EvaluateAnswer(string userId, int wordId, string userAnswer);
    }
}