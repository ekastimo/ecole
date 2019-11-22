using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace App.Areas.Crm.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(
            IContactRepository contactRepository,
            IMapper mapper, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ContactChcViewModel> UpdateChcInformation(ContactChcViewModel model)
        {
            var filter = Builders<Contact>.Filter.Eq(x => x.Id, model.ContactId);
            var update = Builders<Contact>.Update
                .Set(x => x.MetaData, new MetaData
                {
                    ChurchLocation = model.ChurchLocation,
                    CellGroup = model.CellGroup
                });
            await _contactRepository.UpdateAsync(filter, update);
            return model;
        }


        public async Task<ContactViewModel> CreateAsync(ContactViewModel model)
        {
            var emailAddresses = model.Emails.Select(it => it.Value).ToArray();
            var contactExists = await ContactExistsByEmailAsync(emailAddresses);
            if (contactExists)
                throw new ClientFriendlyException($"One of the emails: {emailAddresses.Stringify()} already exists");

            var person = _mapper.Map<Person>(model.Person);
            var emails = model.Emails.Select(it =>
            {
                var rec = _mapper.Map<Email>(it);
                rec.Id = Guid.NewGuid();
                return rec;
            }).ToArray();

            var phones = model.Phones?.Select(it =>
            {
                var rec = _mapper.Map<Phone>(it);
                rec.Id = Guid.NewGuid();
                return rec;
            }).ToArray() ?? new Phone[0];

            var addresses = model.Addresses?.Select(it =>
            {
                var rec = _mapper.Map<Address>(it);
                rec.Id = Guid.NewGuid();
                return rec;
            }).ToArray() ?? new Address[0];

            var identifications = model.Identifications?.Select(it =>
            {
                var rec = _mapper.Map<Identification>(it);
                rec.Id = Guid.NewGuid();
                return rec;
            }).ToArray() ?? new Identification[0];

            var data = new Contact
            {
                Category = ContactCategory.Person,
                Person = person,
                Emails = emails,
                Phones = phones,
                Addresses = addresses,
                Identifications = identifications,
                Tags = model.Tags
            };
            var result = await _contactRepository.CreateAsync(data);
            return _mapper.Map<ContactViewModel>(result);
        }

        public async Task<PersonViewModel> UpdatePerson(Guid contactId, PersonViewModel personModel)
        {
            var result = await _contactRepository.GetByIdAsync(contactId);
            if (result == null)
            {
                throw new ClientFriendlyException($"Invalid contact {contactId}");
            }

            var person = _mapper.Map<Person>(personModel);
            person.Avatar = result.Person.Avatar;
            result.Person = person;
            var updated = await _contactRepository.UpdateAsync(result);
            return _mapper.Map<PersonViewModel>(updated.Person);
        }

        public async Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request)
        {
            return await _contactRepository.SearchMinimalAsync(request);
        }


        public async Task<ContactViewModel> GetByIdAsync(Guid id)
        {
            var result = await _contactRepository.GetByIdAsync(id);

            return _mapper.Map<ContactViewModel>(result);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _contactRepository.DeleteAsync(id);
        }

        public async Task<ContactViewModel> UpdateAsync(ContactViewModel model)
        {
            var contact = _mapper.Map<Contact>(model);
            var result = await _contactRepository.UpdateAsync(contact);
            return _mapper.Map<ContactViewModel>(result);
        }

        public async Task<IEnumerable<ContactViewModel>> SearchAsync(ContactSearchRequest request)
        {
            var result = await _contactRepository.SearchAsync(request);
            return _mapper.Map<IEnumerable<ContactViewModel>>(result);
        }


        public async Task<IEnumerable<ContactViewModel>> GetContactsAsync(List<Guid> guids)
        {
            var result = await _contactRepository.GetContactsAsync(guids);
            return _mapper.Map<IEnumerable<ContactViewModel>>(result);
        }


        public async Task<IDictionary<Guid, MinimalContact>> GetNamesByIdAsync(List<Guid> guids)
        {
            var filter = Builders<Contact>.Filter
                .Where(x => guids.Contains(x.Id));
            var data = await _contactRepository.GetMinimalAsync(filter);
            return data.ToImmutableDictionary(x => x.Id, x => x);
        }

        public async Task<List<MinimalContact>> GetNamesAsync(FilterDefinition<Contact> filter)
        {
            return await _contactRepository.GetMinimalAsync(filter);
        }

        public async Task<ContactViewModel> GetByIdentificationAsync(string idNumber)
        {
            var exception = new Exception($"No person with identification {idNumber}");
            var data = await _contactRepository.GetByIdentificationAsync(idNumber);
            if (data == null)
                throw exception;
            return _mapper.Map<ContactViewModel>(data);
        }

        public async Task<bool> ContactExistsByEmailAsync(params string[] values)
        {
            foreach (var value in values)
            {
                var result = await _contactRepository.ContactExistsByEmailAsync(value);
                if (result)
                    return true;
            }

            return false;
        }

        public async Task<bool> ContactExistsByPhoneAsync(string data)
        {
            return await _contactRepository.ContactExistsByPhoneAsync(data);
        }

        public async Task<bool> ContactExistsByIdentificationAsync(string data)
        {
            return await _contactRepository.ContactExistsByIdentificationAsync(data);
        }
    }
}