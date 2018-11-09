using System;
using System.Threading.Tasks;
using App.Data;
using Core.Exceptions;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories
{
    public interface IEmailRepository
    {
        Task<Models.Email> CreateAsync(Guid parentId, Models.Email entity);
        Task<Models.Email> UpdateAsync(Guid parentId, Models.Email entity);
        Task<int> DeleteAsync(Guid parentId, Guid entytId);
    }

    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Models.Email> CreateAsync(Guid parentId, Models.Email entity)
        {
            entity.Id = Guid.NewGuid();
            var filter = Builders<Models.Contact>.Filter.Eq(x => x.Id, parentId);
            var update = Builders<Models.Contact>.Update
                .AddToSet(x => x.Emails, entity);
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record creation failed {parentId}");
            return entity;
        }

        public async Task<Models.Email> UpdateAsync(Guid parentId, Models.Email entity)
        {
            var builder = Builders<Models.Contact>.Filter;
            var filter = builder.Eq(x => x.Id, parentId)
                         & builder.ElemMatch(x => x.Emails, it => it.Id == entity.Id);
            var update = Builders<Models.Contact>.Update
                .Set(x => x.Emails[-1], entity);
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
                .PullFilter(x => x.Emails, x => x.Id == entityId);

            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException($"record deletion failed {parentId}");
            return (int) result.ModifiedCount;
        }
    }
}