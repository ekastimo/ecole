using App.Data;
using Core.Repositories;

namespace App.Areas.Crm.Repositories.Address
{
    public class AddressRepository : GenericRepository<Models.Address>, IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
 }
}
