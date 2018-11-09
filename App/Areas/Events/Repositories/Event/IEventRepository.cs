using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;
using Core.Repositories;

namespace App.Areas.Events.Repositories.Event
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventRepository : IGenericRepository<Models.Event>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fullQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<Models.Event>> SearchAsync(EventSearchRequest request, bool fullQuery = false);
    }
}