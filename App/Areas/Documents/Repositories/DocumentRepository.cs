using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Documents.Models;
using App.Areas.Events.ViewModels;
using App.Data;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Documents.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Events.Models.Event>> SearchAsync(EventSearchRequest request)
        {
            IQueryable<Events.Models.Event> query = _context.Events;
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
