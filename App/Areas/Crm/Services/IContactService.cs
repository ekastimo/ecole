using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.ViewModels;
using Core.Models;

namespace App.Areas.Crm.Services
{
    
    public interface IContactService
    {
        Task<ContactViewModel> CreateAsync(NewPersonViewModel contact);
        
        Task<ContactViewModel> CreateAsync(NewCompanyViewModel contact);

        Task<ContactViewModel> GetByIdAsync(Guid id);

        Task<int> DeleteAsync(Guid id);

        Task<ContactViewModel> UpdateAsync(ContactViewModel contact);  

        Task<IEnumerable<ContactViewModel>> SearchAsync(ContactSearchRequest request); 

        Task<IEnumerable<ContactViewModel>> GetContactsAsync(List<Guid> guids);
        Task<IDictionary<Guid, MinimalContact>> GetNamesByIdAsync(List<Guid> contactIds);
        Task<ContactViewModel> GetByIdentificationAsync(string id);

        Task<bool> ContactExistsByEmailAsync(params string[] values);

        Task<bool> ContactExistsByPhoneAsync(string data);

        Task<bool> ContactExistsByIdentificationAsync(string data);
        Task<ContactViewModel> CreateAsync(ContactViewModel contact);
        Task<ContactViewModel> UpdatePerson(Guid contactId, PersonViewModel person); 

        Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request);
    }
}
