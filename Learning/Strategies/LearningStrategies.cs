using ZTP_Project.Models;

namespace ZTP_Project.Learning.Strategies
{
    /// <summary>
    /// A factory class that resolves learning strategies from a service provider based on a given mode.
    /// </summary>
    public class LearningStrategies : ILearningStrategies
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningStrategies"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve strategies.</param>
        public LearningStrategies(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public ILearningStrategy GetStrategy(LearningMode mode)
        {
            return mode switch
            {
                LearningMode.Flashcards => _serviceProvider.GetService<FlashcardsStrategy>()!,
                LearningMode.MultipleChoice => _serviceProvider.GetService<MultipleChoiceStrategy>()!,
                LearningMode.FillInTheBlank => _serviceProvider.GetService<FillInTheBlankStrategy>()!,
                _ => throw new ArgumentException("Invalid mode.")
            };
        }
    }
}