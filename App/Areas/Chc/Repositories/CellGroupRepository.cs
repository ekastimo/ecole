using System.Collections.Generic;
using System.Threading.Tasks;
using App.Data;
using Core.Models;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Chc.Repositories
{

    public interface ILocationRepository : IGenericRepository<Models.Location>
    {
        Task<IEnumerable<Models.Location>> SearchAsync(SearchBase request, bool fullQuery = false);
    }
    public class LocationRepository: GenericRepository<Models.Location>, ILocationRepository
    {

        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context):base(context.Locations)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Location>> SearchAsync(SearchBase request, bool fullQuery = false)
        {
            var builder = Builders<Models.Location>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter = builder.Regex(x => x.Name, regex);
            }

            return await _context.Locations
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}
