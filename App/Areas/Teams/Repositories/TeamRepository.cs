using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Teams.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Driver;

namespace App.Areas.Teams.Repositories
{
    public class TeamRepository : GenericRepository<Models.Team>, ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context) : base(context.Teams)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Team>> SearchAsync(TeamSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Models.Team>.Filter;
            var filter = builder.Empty;
           

            return await _context.Teams
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
        public async Task<IEnumerable<Models.Team>> SearchByIdsAsync(List<Guid> teamIds)
        {
            var filter = Builders<Models.Team>.Filter
                .Where(x => teamIds.Contains(x.Id));
            var data = await _context.Teams.Find(filter)
                .ToListAsync();
            return  data;
        }

        public async Task<bool> TeamExistsByNameAsync(string teamName)
        {
            var filter = Builders<Models.Team>.Filter
                .Eq(x => x.Name, teamName);
            return await _context.Teams.Find(filter).AnyAsync();
        }
    }
}