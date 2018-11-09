using System;
using System.Threading.Tasks;
using App.Data;
using Core.Exceptions;
using MongoDB.Driver;

namespace App.Areas.Events.Repositories.Item
{
    public interface IItemRepository
    {
        Task<Models.Item> CreateAsync(Guid parentId, Models.Item entity);
        Task<Models.Item> UpdateAsync(Guid parentId, Models.Item entity);
        Task<int> DeleteAsync(Guid parentId, Guid entytId);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Models.Item> CreateAsync(Guid parentId, Models.Item entity)
        {
            entity.Id = Guid.NewGuid();
            var filter = Builders<Models.Event>.Filter.Eq(x => x.Id, parentId);
            var update = Builders<Models.Event>.Update
                .AddToSet(x => x.Items, entity);
            var result = await _context.Events.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record creation failed {parentId}");
            return entity;
        }

        public async Task<Models.Item> UpdateAsync(Guid parentId, Models.Item entity)
        {
            var builder = Builders<Models.Event>.Filter;
            var filter = builder.Eq(x => x.Id, parentId)
                         & builder.ElemMatch(x => x.Items, it => it.Id == entity.Id);
            var update = Builders<Models.Event>.Update
                .Set(x => x.Items[-1], entity);
            var result = await _context.Events.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record update failed {parentId}");
            return entity;
        }

        public async Task<int> DeleteAsync(Guid parentId, Guid entityId)
        {
            var filter = Builders<Models.Event>.Filter
                .Eq(x => x.Id, parentId);

            var update = Builders<Models.Event>.Update
                .PullFilter(x => x.Items, x => x.Id == entityId);

            var result = await _context.Events.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record deletion failed {parentId}");
            return (int)result.ModifiedCount;
        }
    }
}