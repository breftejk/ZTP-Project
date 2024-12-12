using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZTP_Project.Data.Repositories
{
    /// <summary>
    /// Generic interface for repository operations.
    /// </summary>
    /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its identifier, including specified related entities.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="includes">Expressions for related entities to include.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <returns>An enumerable collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds entities matching the specified predicate, including related entities.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="includes">Expressions for related entities to include.</param>
        /// <returns>An enumerable collection of matching entities.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds a range of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);

        /// <summary>
        /// Saves all changes made in the repository to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}