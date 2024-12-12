using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// <param name="userId">The user's ID.</param>
        /// <param name="languageId">The language ID.</param>
        /// <returns>An enumerable collection of groups.</returns>
        Task<IEnumerable<Group>> GetGroupsWithLanguageAsync(string userId, int languageId);

        /// <summary>
        /// Retrieves a group with its detailed relationships.
        /// </summary>
        /// <param name="groupId">The group's ID.</param>
        /// <returns>The group with details if found; otherwise, null.</returns>
        Task<Group?> GetGroupWithDetailsAsync(int groupId);
    }
}