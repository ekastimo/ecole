using System;
using System.Threading.Tasks;
using App.Data;
using Core.Exceptions;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories
{
    public interface IOccasionRepository
    {
        Task<Models.Occasion> CreateAsync(Guid parentId, Models.Occasion entity);
        Task<Models.Occasion> UpdateAsync(Guid parentId, Models.Occasion entity);
        Task<int> DeleteAsync(Guid parentId, Guid entytId);
    }

    public class OccasionRepository : IOccasionRepository
    {
        private readonly ApplicationDbContext _context;

        public OccasionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Models.Occasion> CreateAsync(Guid parentId, Models.Occasion entity)
        {
            entity.Id = Guid.NewGuid();
            var filter = Builders<Models.Contact>.Filter.Eq(x => x.Id, parentId);
            var update = Builders<Models.Contact>.Update
                .AddToSet(x => x.Occasions, entity);
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record creation failed {parentId}");
            return entity;
        }

        public async Task<Models.Occasion> UpdateAsync(Guid parentId, Models.Occasion entity)
        {
            var builder = Builders<Models.Contact>.Filter;
            var filter = builder.Eq(x => x.Id, parentId)
                         & builder.ElemMatch(x => x.Occasions, it => it.Id == entity.Id);
            var update = Builders<Models.Contact>.Update
                .Set(x => x.Occasions[-1], entity);
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record update failed {parentId}");
            return entity;
        }

        public async Task<int> DeleteAsync(Guid parentId, Guid entityId)
        {
            var filter = Builders<Models.Contact>.Filter
                .Eq(x => x.Id, parentId);

            var update = Builders<Models.Contact>.Update
                .PullFilter(x => x.Occasions, x => x.Id == entityId);

            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record deletion failed contact:{parentId} record:{entityId}");
            return (int) result.ModifiedCount;
        }
    }
}