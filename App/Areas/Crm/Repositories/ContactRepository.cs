using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.ViewModels;
using App.Data;
using Core.Exceptions;
using Core.Models;
using Core.Repositories;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories
{
    public interface IContactRepository : IGenericRepository<Models.Contact>
    {
        Task<IEnumerable<Models.Contact>> SearchAsync(ContactSearchRequest request, bool fullQuery = false);
        Task<bool> ContactExistsByIdAsync(Guid id);
        Task<bool> ContactExistsByIdentificationAsync(string nationalIdNumber);
        Task<bool> ContactExistsByEmailAsync(string email);
        Task<bool> ContactExistsByPhoneAsync(string phone);

        Task<IEnumerable<Models.Contact>> GetContactsAsync(List<Guid> guids);
        Task<Models.Contact> GetByIdentificationAsync(string idNumber);
        Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request);
        Task<List<MinimalContact>> GetMinimalAsync(FilterDefinition<Models.Contact> filter);
        Task<long> UpdateAsync(FilterDefinition<Models.Contact> filter, UpdateDefinition<Models.Contact> update);
    }

    public class ContactRepository : GenericRepository<Models.Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context) : base(context.Contacts)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Contact>> SearchAsync(ContactSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Models.Contact>.Filter;
            var filter = builder.Empty;
            if (request.Id.HasValue)
            {
                filter &= builder.Eq(x => x.Id, request.Id);
            }

            if (!string.IsNullOrEmpty(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter &= filter & builder.Or(
                              builder.Regex(x => x.Person.FirstName, regex),
                              builder.Regex(x => x.Person.LastName, regex),
                              builder.Regex(x => x.Person.MiddleName, regex),
                              builder.Regex(x => x.Company.Name, regex)
                          );
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                filter &= builder.ElemMatch(x => x.Emails, x => x.Value == request.Email);
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                filter &= filter & builder.ElemMatch(x => x.Phones, x => x.Value == request.Phone);
            }

            if (!string.IsNullOrEmpty(request.ChurchLocation))
            {
                filter &= filter & builder.Eq(x => x.MetaData.ChurchLocation, request.ChurchLocation);
            }

            if (!string.IsNullOrEmpty(request.CellGroup))
            {
                filter &= filter & builder.Eq(x => x.MetaData.CellGroup, request.CellGroup);
            }

            return await _context.Contacts
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }

        public async Task<bool> ContactExistsByIdAsync(Guid id)
        {
            var filter = Builders<Models.Contact>.Filter
                .Eq(it => it.Id, id);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<bool> ContactExistsByIdentificationAsync(string idNumber)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Identifications, x => x.Value == idNumber);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<bool> ContactExistsByEmailAsync(string email)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Emails, x => x.Value == email);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<bool> ContactExistsByPhoneAsync(string phone)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Phones, x => x.Value == phone);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<IEnumerable<Models.Contact>> GetContactsAsync(List<Guid> guidList)
        {
            var filter = Builders<Models.Contact>.Filter.In(x => x.Id, guidList);
            return await _context.Contacts.Find(filter).ToListAsync();
        }

        public async Task<Models.Contact> GetByIdentificationAsync(string idNumber)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Identifications, x => x.Value == idNumber);
            return await _context.Contacts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<MinimalContact>> GetMinimalAsync(FilterDefinition<Models.Contact> filter)
        {
            var query = _context.Contacts.AsQueryable()
                .Select(x => new MinimalContact
                {
                    Id = x.Id,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                    MiddleName = x.Person.MiddleName,
                    Avatar = x.Person.Avatar,
                    Email = x.Emails[0].Value,
                    Phone = x.Phones[0].Value
                });
            return await query.ToListAsync();
        }

        public async Task<long> UpdateAsync(FilterDefinition<Models.Contact> filter, UpdateDefinition<Models.Contact> update)
        {
            var result = await _context.Contacts.UpdateOneAsync(filter, update);
            if (result.ModifiedCount != 1)
                throw new ClientFriendlyException("record update failed");
            return result.ModifiedCount;
        }

        public async Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request)
        {
            var builder = Builders<Models.Contact>.Filter;

            var filter = builder.Empty;

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var regex = new BsonRegularExpression(request.Query, "i");
                filter = builder.Or(
                    builder.Regex(x => x.Person.FirstName, regex),
                    builder.Regex(x => x.Person.LastName, regex),
                    builder.Regex(x => x.Person.MiddleName, regex),
                    builder.Regex(x => x.Company.Name, regex)
                );
            }

            return await GetMinimalAsync(filter);
        }
    }
}