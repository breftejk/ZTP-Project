using ZTP_Project.Models;

namespace ZTP_Project.Learning.Strategies
{
    /// <summary>
    /// Represents a factory that provides learning strategies based on a specified mode.
    /// </summary>
    public interface ILearningStrategyFactory
    {
        /// <summary>
        /// Retrieves a learning strategy instance for a given learning mode.
        /// </summary>
        /// <param name="mode">The learning mode.</param>
        /// <returns>An instance of a learning strategy.</returns>
        ILearningStrategy GetStrategy(LearningMode mode);
    }
}