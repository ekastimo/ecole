using App.Data;
using Core.Repositories;

namespace App.Areas.Crm.Repositories.Email
{
    
    public class EmailRepository : GenericRepository<Models.Email>, IEmailRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
 
        }
}
