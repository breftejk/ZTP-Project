using ZTP_Project.Models;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Repository for managing languages.
    /// </summary>
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public LanguageRepository(ApplicationDbContext context) : base(context) { }
    }
}