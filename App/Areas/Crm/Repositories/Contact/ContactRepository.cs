using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Areas.Crm.Repositories.Contact
{
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
                filter = filter & builder.Eq(x => x.Id, request.Id);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                var regex = new BsonRegularExpression(request.Name, "i");
                filter = filter & builder.Or(
                             builder.Regex(x => x.Person.FirstName, regex),
                             builder.Regex(x => x.Person.OtherNames, regex),
                             builder.Regex(x => x.Company.Name, regex)
                         );
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                filter = filter & builder.ElemMatch(x => x.Emails, x => x.Address == request.Email);
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                filter = filter & builder.ElemMatch(x => x.Phones, x => x.Number == request.Phone);
            }

            return await _context.Contacts
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Limit)
                .ToListAsync();
        }

        public async Task<bool> ContactExistsByIdentificationAsync(string idNumber)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Identifications, x => x.Number == idNumber);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<bool> ContactExistsByEmailAsync(string email)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Emails, x => x.Address == email);
            return await _context.Contacts.Find(filter).AnyAsync();
        }

        public async Task<bool> ContactExistsByPhoneAsync(string phone)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Phones, x => x.Number == phone);
            return await _context.Contacts.Find(filter).AnyAsync();

//           var result = await (from x in _context.Contacts.AsQueryable()
//                where x.Emails.Any(child =>
//                    child.Address == phone)
//                select x).AnyAsync();
//            return result;
        }

        public async Task<IEnumerable<Models.Contact>> GetContactsAsync(List<Guid> guids)
        {
            var filter = Builders<Models.Contact>.Filter.In(x => x.Id, guids);
            return await _context.Contacts.Find(filter).ToListAsync();
        }

        public async Task<Models.Contact> GetByIdentificationAsync(string idNumber)
        {
            var filter = Builders<Models.Contact>.Filter
                .ElemMatch(x => x.Identifications, x => x.Number == idNumber);
            return await _context.Contacts.Find(filter).FirstOrDefaultAsync();
        }
    }
}