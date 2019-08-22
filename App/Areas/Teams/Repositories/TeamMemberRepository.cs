using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Teams.Models;
using App.Areas.Teams.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Driver;

namespace App.Areas.Teams.Repositories
{
    public class TeamMemberRepository : GenericRepository<TeamMember>, ITeamMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamMemberRepository(ApplicationDbContext context) : base(context.TeamMembers)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamMember>> SearchAsync(TeamMemberSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<TeamMember>.Filter;
            var filter = builder.Empty;
            if (request.ContactId.HasValue)
            {
                filter = filter & builder.Eq(x => x.ContactId, request.ContactId);
            }

            if (request.TeamId.HasValue)
            {
                filter = filter & builder.Eq(x => x.TeamId, request.TeamId);
            }

            if (request.Roles?.Any() ?? false)
            {
                filter = filter & builder.Where(q =>request.Roles.Contains(q.Role));
                
            }
            return await _context.TeamMembers.Find(filter).ToListAsync();
        }

        public async Task<bool> MemberExistsByAsync(Guid contactId)
        {
            var filter = Builders<TeamMember>.Filter
                .Eq(x => x.ContactId, contactId);
            return await _context.TeamMembers.Find(filter).AnyAsync();
        }
    }
}