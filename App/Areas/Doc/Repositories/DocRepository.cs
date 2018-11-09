using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Doc.ViewModels;
using App.Data;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace App.Areas.Doc.Repositories
{
    public class DocRepository : GenericRepository<Models.Doc>, IDocRepository
    {
        private readonly ApplicationDbContext _context;

        public DocRepository(ApplicationDbContext context) : base(context.Docs)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Doc>> SearchAsync(DocSearchRequest request, bool fullQuery = false)
        {
            IQueryable<Models.Doc> query = _context.Docs.AsQueryable();
            if (request.ContactId.HasValue)
                query = query.Where(q => q.CreatedBy.Equals(request.ContactId));

            if (request.Id.HasValue)
                query = query.Where(q => q.Id.Equals(request.Id));

            if (string.IsNullOrWhiteSpace(request.Query))
                query = query.Where(q =>
                    q.Description.Contains(request.Query) ||
                    q.FileName.Contains(request.Query) ||
                    q.OriginalFileName.Contains(request.Query)
                );

            return await query
                .Skip(request.Skip)
                .Take(request.Limit)
                .AsNoTracking().ToListAsync();
        }
    }
}