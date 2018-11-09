using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, int skip = 0, int limit = 100);

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(Guid id);
     

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity);


        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entities"></param> 
        /// <returns></returns>
        Task<IEnumerable<TEntity>> CreateBatchAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Determines if the condition in the expression returns any results
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<bool> MatchesConditionAsync(FilterDefinition<TEntity> filter);
    }
}