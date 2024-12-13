using System.Threading.Tasks;
using ZTP_Project.Models;
using System.Collections.Generic;

namespace ZTP_Project.Learning
{
    public interface ILearningFacade
    {
        Task<IEnumerable<Group>> GetGroupsWithLanguageAsync(string userId, int languageId);
        Task<LearningResult> EvaluateAnswerAsync(string userId, int wordId, string userAnswer, LearningMode mode);
        Task<IEnumerable<Word>> GetWordsToRepeatAsync(string userId, int languageId);
        Task<IEnumerable<Word>> GetAllWordsAsync(int languageId);
        Task<IEnumerable<Word>> GetGroupWordsAsync(int groupId);
        Task<List<string>> GetMultipleChoiceOptionsAsync(int wordId);
    }
}