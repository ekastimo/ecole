using App.Data;
using Core.Repositories;

namespace App.Areas.Crm.Repositories.Phone
{
    public class PhoneRepository : GenericRepository<Models.Phone>, IPhoneRepository
    {
        public PhoneRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
