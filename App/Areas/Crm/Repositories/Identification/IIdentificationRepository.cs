using System.Threading.Tasks;
using Core.Repositories;

namespace App.Areas.Crm.Repositories.Identification
{
    public interface IIdentificationRepository : IGenericRepository<Models.Identification>
    { 
        Task<bool> IdentificationExistsAsync(string idNumber);
        Task<Models.Identification> GetByIndentificationNumberAsync(string idNumber);
    }
}
