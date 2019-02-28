using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Contact;
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

        public async Task<ContactViewModel> CreateAsync(NewPersonViewModel model)
        {
            var contactExists = await ContactExistsByEmailAsync(model.Email);
            if (contactExists)
                throw new ClientFriendlyException($"Contact ( Email:{model.Email}) already exists");

            var data = new Contact
            {
                Category = ContactCategory.Person,
                Person = new Person
                {
                    FirstName = model.FirstName,
                    MiddleName = model.LastName,
                    LastName = model.MiddleName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Salutation = model.Salutation,
                    CivilStatus = model.CivilStatus,
                    About = model.About,
                    Avatar = model.Avatar
                }
            };
            LoadProperties(data, model);
            var result = await _contactRepository.CreateAsync(data);
            return _mapper.Map<ContactViewModel>(result);
        }

        public async Task<ContactChcViewModel> UpdateChcInformation(ContactChcViewModel model)
        {
            var filter = Builders<Contact>.Filter.Eq(x => x.Id, model.ContactId);
            var update = Builders<Contact>.Update
                .Set(x => x.ChurchLocation, model.ChurchLocation)
                .Set(x => x.CellGroup, model.CellGroup);
            await _contactRepository.UpdateAsync(filter, update);
            return model;
        }


        public async Task<ContactViewModel> CreateAsync(ContactViewModel model)
        {
            var emailAddresses = model.Emails.Select(it => it.Address).ToArray();
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

        public async Task<ContactViewModel> UpdatePerson(Guid contactId, PersonViewModel personModel)
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
            return _mapper.Map<ContactViewModel>(updated);
        }

        public async Task<IEnumerable<MinimalContact>> SearchMinimalAsync(SearchBase request)
        {
            return await _contactRepository.SearchMinimalAsync(request);
        }

        private static (bool valid, Identification identification) ValidateId(NewContactViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.IdentificationNumber))
            {
                return (false, null);
            }

            if (model.IdentificationCategory == null ||
                model.IdentificationValidFrom == null ||
                model.IdentificationValidTo == null)
            {
                return (false, null);
            }

            var identification = new Identification
            {
                Id = Guid.NewGuid(),
                Category = model.IdentificationCategory.Value,
                Number = model.IdentificationNumber,
                StartDate = model.IdentificationValidFrom.Value,
                ExpiryDate = model.IdentificationValidTo.Value,
                IsPrimary = true
            };
            return (true, identification);
        }

        private static void LoadProperties(Contact contact, NewContactViewModel model)
        {
            var idData = ValidateId(model);
            bool IsDefined(string value) => !string.IsNullOrWhiteSpace(value);

            contact.Identifications = idData.valid
                ? new[]
                {
                    idData.identification
                }
                : new Identification[] { };

            contact.Emails = IsDefined(model.Email)
                ? new[]
                {
                    new Email
                    {
                        Id = Guid.NewGuid(),
                        Category = EmailCategory.Personal,
                        Address = model.Email,
                        IsPrimary = true
                    }
                }
                : new Email[] { };
            contact.Phones = IsDefined(model.Phone)
                ? new[]
                {
                    new Phone
                    {
                        Id = Guid.NewGuid(),
                        Category = PhoneCategory.Mobile,
                        Number = model.Phone,
                        IsPrimary = true
                    }
                }
                : new Phone[] { };
            var hasTags = model.Tags?.Any() ?? false;
            contact.Tags = hasTags
                ? model.Tags.ToArray()
                : new string[] { };
            contact.Addresses= new Address[0];
        }

        public async Task<ContactViewModel> CreateAsync(NewCompanyViewModel model)
        {
            var data = new Contact
            {
                Category = ContactCategory.Company,
                Company = new Company
                {
                    Name = model.Name
                }
            };
            LoadProperties(data, model);

            var contactExists = await ContactExistsByIdentificationAsync(model.IdentificationNumber);
            if (contactExists)
                throw new Exception($"Company ( NIN:{model.IdentificationNumber}) already exists");

            var result = await _contactRepository.CreateAsync(data);
            return _mapper.Map<ContactViewModel>(result);
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
            var filter = Builders<Models.Contact>.Filter
                .Where(x => guids.Contains(x.Id));
            var data = await _contactRepository.GetNamesAsync(filter);
            return data.ToImmutableDictionary(x => x.Id, x => x);
        }

        public async Task<List<MinimalContact>> GetNamesAsync(FilterDefinition<Contact> filter)
        {
            return await _contactRepository.GetNamesAsync(filter); ;
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