using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;
using Core.Models;
using MongoDB.Driver;

namespace App.Areas.Crm.Services
{
    
    public interface IContactService
    {
        Task<ContactChcViewModel> UpdateChcInformation(ContactChcViewModel model);

        Task<ContactViewModel> GetByIdAsync(Guid id);

        Task<int> DeleteAsync(Guid id);

        Task<ContactViewModel> UpdateAsync(ContactViewModel contact);  

        Task<IEnumerable<ContactViewModel>> SearchAsync(ContactSearchRequest request); 

        Task<IEnumerable<ContactViewModel>> GetContactsAsync(List<Guid> guidList);
        Task<IDictionary<Guid, MinimalContact>> GetNamesByIdAsync(List<Guid> contactIds);
        Task<List<MinimalContact>> GetNamesAsync(FilterDefinition<Contact> filter);
        Task<ContactViewModel> GetByIdentificationAsync(string id);

        Task<bool> ContactExistsByEmailAsync(params string[] values);

        Task<bool> ContactExistsByPhoneAsync(string data);

        Task<bool> ContactExistsByIdentificationAsync(string data);
        Task<ContactViewModel> CreateAsync(ContactViewModel contact);
        Task<PersonViewModel> UpdatePerson(Guid contactId, PersonViewModel person); 

        Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request);
    }
}
