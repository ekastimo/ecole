using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Groups.Models;
using App.Areas.Groups.ViewModels;
using App.Data;
using Core.Repositories;
using MongoDB.Driver;

namespace App.Areas.Groups.Repositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<IEnumerable<Member>> SearchAsync(MemberSearchRequest request, bool fullQuery = false);
        Task<bool> ExistsByIdAsync(Guid contactId);
    }

    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context) : base(context.GroupMembers)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> SearchAsync(MemberSearchRequest request, bool fullQuery = false)
        {
            var builder = Builders<Member>.Filter;
            var filter = builder.Empty;
            if (request.ContactId.HasValue)
            {
                filter &= builder.Eq(x => x.ContactId, request.ContactId);
            }

            if (request.GroupId.HasValue)
            {
                filter &= builder.Eq(x => x.GroupId, request.GroupId);
            }

            if (request.Roles?.Any() ?? false)
            {
                filter &= builder.Where(q =>request.Roles.Contains(q.Role));
                
            }
            return await _context.GroupMembers.Find(filter).ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(Guid contactId)
        {
            var filter = Builders<Member>.Filter
                .Eq(x => x.ContactId, contactId);
            return await _context.GroupMembers.Find(filter).AnyAsync();
        }
    }
}