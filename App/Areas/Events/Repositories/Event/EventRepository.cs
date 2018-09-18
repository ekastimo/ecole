using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;
using App.Data;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Events.Repositories.Event
{
    public class EventRepository : GenericRepository<Models.Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
 
        public async Task<IEnumerable<Models.Event>> SearchAsync(EventSearchRequest request)
        {
            IQueryable<Models.Event> query = _context.Events;
            if (request.ContactId.HasValue)
                query = query.Where(q => q.CreatorId.Equals(request.ContactId));

            if (request.Id.HasValue)
                query = query.Where(q => q.Id.Equals(request.Id));

            return await query
                .Skip(request.Skip)
                .Take(request.Limit)
                .AsNoTracking().ToListAsync();
        }
    }
}
