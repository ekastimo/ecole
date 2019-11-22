using System.Collections.Generic;
using System.Threading.Tasks;
using App.Data;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Tags
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Tag>> SearchAsync(TagSearchRequest request);
    }

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context) : base(context.Tags)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> SearchAsync(TagSearchRequest request)
        {
            var builder = Builders<Tag>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter &= builder.Regex(x => x.Name, regex);
            }

            if (request.Categories?.Length>0)
            {
           
                filter &= builder.In(x => x.Category, request.Categories);
            }

            return await _context.Tags
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }
    }
}