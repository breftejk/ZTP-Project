using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Interface for group-specific repository operations.
    /// </summary>
    public interface IGroupRepository : IRepository<Group>
    {
        /// <summary>
        /// Retrieves groups associated with a specific user and language.
        /// </summary>
        Task<IEnumerable<Group>> GetGroupsWithLanguageAsync(string userId, int languageId);

        /// <summary>
        /// Retrieves a group with its detailed relationships.
        /// </summary>
        Task<Group?> GetGroupWithDetailsAsync(int groupId);
    }
}