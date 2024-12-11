using ZTP_Project.Data;
using ZTP_Project.Models;

namespace ZTP_Project.Repositories
{
    /// <summary>
    /// Repository for managing words.
    /// </summary>
    public class WordRepository : Repository<Word>, IWordRepository
    {
        public WordRepository(ApplicationDbContext context) : base(context) { }
    }
}