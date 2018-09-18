using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.ViewModels;
using App.Data;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Crm.Repositories.Contact
{
    public class ContactRepository : GenericRepository<Models.Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        private IQueryable<Models.Contact> MakeFullQuery(IQueryable<Models.Contact> queryable)
        {
            return queryable
                    .Include(it => it.Person)
                    .Include(it => it.Company)
                    .Include(it => it.Identifications)
                    .Include(it => it.Emails)
                    .Include(it => it.Phones)
                    .Include(it => it.Addresses)
                    .Include(it => it.ContactTags)
                    .Include(it => it.ContactTags)
                ;
        }

        private IQueryable<Models.Contact> MakeMiniQuery(IQueryable<Models.Contact> queryable)
        {
            return queryable
                .Include(it => it.Person)
                .Include(it => it.Company)
                .Include(it => it.Emails)
                .Include(it => it.Phones);
        }

        public async Task<IEnumerable<Models.Contact>> SearchAsync(ContactSearchRequest request, bool fullQuery = false)
        {
            IQueryable<Models.Contact> query = _context.Contacts;

            if (request.Id.HasValue)
                query = query.Where(q => q.Id.Equals(request.Id));

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(
                    q =>
                        q.Person.FirstName.Contains(request.Name) ||
                        q.Person.OtherNames.Contains(request.Name) ||
                        q.Company.Name.Contains(request.Name)
                );
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                var ids = await _context.Emails.Where(it => it.Address.Contains(request.Email))
                    .Select(it => it.ContactId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(q => ids.Contains(q.Id));
                }
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                var ids = await _context.Phones.Where(it => it.Number.Contains(request.Phone))
                    .Select(it => it.ContactId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(q => ids.Contains(q.Id));
                }
            }

//            if (request.Tags.Any())
//            {
//                query = query.Where(
//                    q =>q.
//                );
//            }

            query = fullQuery ? MakeFullQuery(query) : MakeMiniQuery(query);
            var contacts = await query
                .Skip(request.Skip)
                .Take(request.Limit)
                .ToListAsync();
            return contacts;
        }


        public new async Task<Models.Contact> GetByIdAsync(Guid id)
        {
            return await MakeFullQuery(_context.Contacts)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<bool> ContactExistsByIdentificationAsync(string nationalIdNumber)
        {
            return await _context.Identifications.AnyAsync(it => it.Number.Equals(nationalIdNumber));
        }

        public async Task<bool> ContactExistsByEmailAsync(string email)
        {
            return await _context.Emails.AnyAsync(it => it.Address.Equals(email));
        }

        public async Task<bool> ContactExistsByPhoneAsync(string phone)
        {
            return await _context.Phones.AnyAsync(it => it.Number.Equals(phone));
        }

        public async Task<IEnumerable<Models.Contact>> GetContactsAsync(List<Guid> guids)
        {
            return await MakeMiniQuery(_context.Contacts)
                .Where(q => guids.Contains(q.Id))
                .ToListAsync();
        }
    }
}