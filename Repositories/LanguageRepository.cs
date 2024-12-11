using ZTP_Project.Data;
using ZTP_Project.Models;

namespace ZTP_Project.Repositories
{
    /// <summary>
    /// Repository for managing languages.
    /// </summary>
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApplicationDbContext context) : base(context) { }
    }
}