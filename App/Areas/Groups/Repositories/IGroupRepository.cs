using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Groups.ViewModels;
using Core.Repositories;

namespace App.Areas.Groups.Repositories
{
    public interface IGroupRepository : IGenericRepository<Models.Group>
    {
        Task<IEnumerable<Models.Group>> SearchAsync(GroupSearchRequest request, bool fullQuery = false);

        Task<bool> ExistsByNameAsync(string name);

        Task<IEnumerable<Models.Group>> SearchByIdsAsync(List<Guid> teamIds);
    }
}
