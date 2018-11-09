using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;
using Core.Repositories;

namespace App.Areas.Events.Repositories.Todo
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITodoRepository : IGenericRepository<Models.Todo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<Models.Todo>> SearchAsync(TodoSearchRequest request, bool fullQuery = false);
    }
}