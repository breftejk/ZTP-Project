using System.Linq.Expressions;

namespace ZTP_Project.Repositories
{
    /// <summary>
    /// Generic interface for repository operations.
    /// </summary>
    /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        Task SaveChangesAsync();
    }
}