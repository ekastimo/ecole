using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Driver;

namespace App.Areas.Events.Repositories.Todo
{
    public class TodoRepository : GenericRepository<Models.Todo>, ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context) : base(context.Todos)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Todo>> SearchAsync(TodoSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Models.Todo>.Filter;
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

            return await _context.Todos
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}