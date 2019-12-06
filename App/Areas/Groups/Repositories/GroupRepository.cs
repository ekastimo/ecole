using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Groups.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Groups.Repositories
{
    public class GroupRepository : GenericRepository<Models.Group>, IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context) : base(context.Groups)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Group>> SearchAsync(GroupSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Models.Group>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter = builder.Or(
                    builder.Regex(x => x.Name, regex),
                    builder.Regex(x => x.Description, regex),
                    builder.Regex(x => x.Tag, regex)
                );
            }

            return await _context.Groups
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
        public async Task<IEnumerable<Models.Group>> SearchByIdsAsync(List<Guid> teamIds)
        {
            var filter = Builders<Models.Group>.Filter
                .Where(x => teamIds.Contains(x.Id));
            var data = await _context.Groups.Find(filter)
                .ToListAsync();
            return  data;
        }

        public async Task<bool> ExistsByNameAsync(string teamName)
        {
            var filter = Builders<Models.Group>.Filter
                .Eq(x => x.Name, teamName);
            return await _context.Groups.Find(filter).AnyAsync();
        }
    }
}