using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing groups and their relationships.
    /// </summary>
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public GroupRepository(ApplicationDbContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<IEnumerable<Group>> GetGroupsWithLanguageAsync(string userId, int languageId)
        {
            return await _dbSet
                .Include(g => g.Language)
                .Where(g => g.UserId == userId && g.LanguageId == languageId)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Group?> GetGroupWithDetailsAsync(int groupId)
        {
            return await _dbSet
                .Include(g => g.GroupWords)
                .ThenInclude(gw => gw.Word)
                .Include(g => g.Language)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }
    }
}