using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Driver;

namespace App.Areas.Events.Repositories.Event
{
    public class EventRepository : GenericRepository<Models.Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context.Events)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Models.Event>> SearchAsync(EventSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Models.Event>.Filter;
            var filter = builder.Empty;
            if (request.Id.HasValue)
            {
                filter = filter & builder.Eq(x => x.Id, request.Id);
            }

            if (!string.IsNullOrEmpty(request.Query))
            {
                filter = filter & builder.Or(
                             builder.Regex(x => x.Name, request.Query),
                             builder.Regex(x => x.Details, request.Query),
                             builder.AnyEq(x => x.Tags, request.Query)
                         );
            }
            
            
            if (fullQuery)
            {
                return await _context.Events.Find(filter)
                    .Skip(request.Skip)
                    .Limit(request.Limit)
                    .ToListAsync();
            }
            var projection = Builders<Models.Event>.Projection
                .Exclude(x => x.Items)
                .Exclude(x => x.Images);
            return await _context.Events.Find(filter)
                .Project<Models.Event>(projection)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}