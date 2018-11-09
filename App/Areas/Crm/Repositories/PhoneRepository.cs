using System;
using System.Threading.Tasks;
using App.Data;
using Core.Exceptions;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories
{
    public interface IPhoneRepository
    {
        Task<Models.Phone> CreateAsync(Guid parentId, Models.Phone entity);
        Task<Models.Phone> UpdateAsync(Guid parentId, Models.Phone entity);
        Task<int> DeleteAsync(Guid parentId, Guid entytId);
    }

    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _context;

        public PhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Models.Phone> CreateAsync(Guid parentId, Models.Phone entity)
        {
            entity.Id = Guid.NewGuid();
            var filter = Builders<Models.Contact>.Filter.Eq(x => x.Id, parentId);
            var update = Builders<Models.Contact>.Update
                .AddToSet(x => x.Phones, entity);
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record creation failed {parentId}");
            return entity;
        }

        public async Task<Models.Phone> UpdateAsync(Guid parentId, Models.Phone entity)
        {
            var builder = Builders<Models.Contact>.Filter;
            var filter = builder.Eq(x => x.Id, parentId)
                         & builder.ElemMatch(x => x.Phones, it => it.Id == entity.Id);
            var update = Builders<Models.Contact>.Update
                .Set(x => x.Phones[-1], entity);
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
                .PullFilter(x => x.Phones, x => x.Id == entityId);

            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record deletion failed {parentId}");
            return (int)result.ModifiedCount;
        }
    }
}