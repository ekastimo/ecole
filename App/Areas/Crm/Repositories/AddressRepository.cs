using System;
using System.Threading.Tasks;
using App.Data;
using Core.Exceptions;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories
{
    public interface IAddressRepository
    {
        Task<Models.Address> CreateAsync(Guid parentId, Models.Address entity);
        Task<Models.Address> UpdateAsync(Guid parentId, Models.Address entity);
        Task<int> DeleteAsync(Guid parentId, Guid entityId);
    }

    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Models.Address> CreateAsync(Guid parentId, Models.Address entity)
        {
            entity.Id = Guid.NewGuid();
            var filter = Builders<Models.Contact>.Filter.Eq(x => x.Id, parentId);
            var update = Builders<Models.Contact>.Update
                .AddToSet(x => x.Addresses, entity);
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record creation failed {parentId}");
            return entity;
        }

        public async Task<Models.Address> UpdateAsync(Guid parentId, Models.Address entity)
        {
            var builder = Builders<Models.Contact>.Filter;
            var filter = builder.Eq(x => x.Id, parentId)
                         & builder.ElemMatch(x => x.Addresses, it => it.Id == entity.Id);
            var update = Builders<Models.Contact>.Update
                .Set(x => x.Addresses[-1], entity);
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
                .PullFilter(x => x.Addresses, x => x.Id == entityId);

            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record deletion failed {parentId}");
            return (int)result.ModifiedCount;
        }
    }
}