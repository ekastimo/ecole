using System.Collections.Generic;
using System.Threading.Tasks;
using App.Data;
using Core.Models;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Chc.Repositories
{

    public interface ICellGroupRepository : IGenericRepository<Models.CellGroup>
    {
        Task<IEnumerable<Models.CellGroup>> SearchAsync(SearchBase request, bool fullQuery = false);
    }
    public class CellGroupRepository: GenericRepository<Models.CellGroup>, ICellGroupRepository
    {

        private readonly ApplicationDbContext _context;

        public CellGroupRepository(ApplicationDbContext context):base(context.CellGroups)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.CellGroup>> SearchAsync(SearchBase request, bool fullQuery = false)
        {
            var builder = Builders<Models.CellGroup>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter = builder.Regex(x => x.Name, regex);
            }

            return await _context.CellGroups
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}
