using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing languages.
    /// </summary>
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApplicationDbContext context) : base(context) { }
    }
}