using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.ViewModels;
using Core.Repositories;

namespace App.Areas.Crm.Repositories.Contact
{
    public interface IContactRepository : IGenericRepository<Models.Contact>
    {
        Task<IEnumerable<Models.Contact>> SearchAsync(ContactSearchRequest request,bool fullQuery=false);
        Task<bool> ContactExistsByIdentificationAsync(string nationalIdNumber);
        Task<bool> ContactExistsByEmailAsync(string email);
        Task<bool> ContactExistsByPhoneAsync(string phone);
        Task<IEnumerable<Models.Contact>> GetContactsAsync(List<Guid> guids);
        Task<Models.Contact> GetByIdentificationAsync(string idNumber);
    }
}