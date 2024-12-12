using ZTP_Project.Models;

namespace ZTP_Project.Learning.Strategies
{
    /// <summary>
    /// Interface defining a factory for resolving learning strategies based on a given mode.
    /// </summary>
    public interface ILearningStrategies
    {
        /// <summary>
        /// Retrieves a learning strategy instance for a given learning mode.
        /// </summary>
        /// <param name="mode">The learning mode to resolve the strategy for.</param>
        /// <returns>An instance of a learning strategy implementing <see cref="ILearningStrategy"/>.</returns>
        ILearningStrategy GetStrategy(LearningMode mode);
    }
}