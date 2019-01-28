using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Teams.ViewModels;
using Core.Repositories;

namespace App.Areas.Teams.Repositories
{
    public interface ITeamRepository : IGenericRepository<Models.Team>
    {
        Task<IEnumerable<Models.Team>> SearchAsync(TeamSearchRequest request, bool fullQuery = false);

        Task<bool> TeamExistsByNameAsync(string teamName);

        Task<IEnumerable<Models.Team>> SearchByIdsAsync(List<Guid> teamIds);
    }
}
