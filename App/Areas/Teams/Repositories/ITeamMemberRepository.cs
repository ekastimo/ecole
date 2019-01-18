using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Teams.ViewModels;
using Core.Repositories;

namespace App.Areas.Teams.Repositories
{
    public interface ITeamMemberRepository : IGenericRepository<Models.TeamMember>
    {
        Task<IEnumerable<Models.TeamMember>> SearchAsync(TeamMemberSearchRequest request, bool fullQuery = false);
        Task<bool> MemberExistsByAsync(Guid contactId);
    }
}
