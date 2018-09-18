using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Repositories.Identification;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace App.Areas.Crm.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IIdentificationRepository _identificationRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(
            IContactRepository contactRepository,
            IIdentificationRepository identificationRepository,
            IMapper mapper, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _identificationRepository = identificationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ContactViewModel> CreateAsync(NewPersonViewModel model)
        {
            var data = new Contact
            {
                Category = ContactCategory.Person,
                Person = new Person
                {
                    FirstName = model.FirstName,
                    OtherNames = model.OtherNames,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Salutation = model.Salutation,
                    CivilStatus = model.CivilStatus,
                    About = model.About,
                    Avatar = model.Avatar
                }
            };
            LoadProperties(data, model);
            var contactExists = await ContactExistsAsync(model.IdentificationNumber);
            if (contactExists)
                throw new ClientFriendlyException($"Person ( NIN:{model.IdentificationNumber}) already exists");

            var result = await _contactRepository.CreateAsync(data);

            return _mapper.Map<ContactViewModel>(result);
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
                ? new List<Identification>
                {
                    idData.identification
                }
                : null;

            contact.Emails = IsDefined(model.Email)
                ? new List<Email>
                {
                    new Email
                    {
                        Category = EmailCategory.Private,
                        Address = model.Email,
                        IsPrimary = true
                    }
                }
                : null;
            contact.Phones = IsDefined(model.Phone)
                ? new List<Phone>
                {
                    new Phone
                    {
                        Category = PhoneCategory.Mobile,
                        Number = model.Phone,
                        IsPrimary = true
                    }
                }
                : null;
            var hasTags = model.Tags?.Any() ?? false;
            contact.ContactTags = hasTags
                ? model.Tags.Select(it => new ContactCTag {Tag = new CTag {Id = it}}).ToList()
                : null;
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

            var contactExists = await ContactExistsAsync(model.IdentificationNumber);
            if (contactExists)
                throw new Exception($"Company ( NIN:{model.IdentificationNumber}) already exists");

            var result = await _contactRepository.CreateAsync(data);

            return _mapper.Map<ContactViewModel>(result);
        }

        public async Task<ContactViewModel> GetByIdAsync(Guid id)
        {
            var result = await _contactRepository.GetByIdAsync(id);
            var model = _mapper.Map<ContactViewModel>(result);
           // model.
            return model;
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

        public async Task<IEnumerable<MinimalContact>> SearchAsync(ContactSearchRequest request)
        {
            var result = await _contactRepository.SearchAsync(request);
            return result.Select(contact =>
            {
                var miniContact = _mapper.Map<MinimalContact>(contact.Person);
                miniContact.Category = contact.Category;
                miniContact.Email = contact.Emails?.FirstOrDefault(it => it.IsPrimary)?.Address;
                miniContact.Phone = contact.Phones?.FirstOrDefault(it => it.IsPrimary)?.Number;
                return miniContact;
            }).ToList();
        }

        private async Task<bool> ContactExistsAsync(string number)
        {
            return await _contactRepository.ContactExistsByIdentificationAsync(number);
        }

        public async Task<IEnumerable<MinimalContact>> GetContactsAsync(List<Guid> guids)
        {
            var result = await _contactRepository.GetContactsAsync(guids);
            return result.Select(contact =>
            {
                var miniContact = _mapper.Map<MinimalContact>(contact.Person);
                miniContact.Category = contact.Category;
                miniContact.Email = contact.Emails?.FirstOrDefault(it => it.IsPrimary)?.Address;
                miniContact.Phone = contact.Phones?.FirstOrDefault(it => it.IsPrimary)?.Number;
                return miniContact;
            }).ToList();
        }

        public async Task<ContactViewModel> GetByIdentificationAsync(string idNumber)
        {
            var exception = new Exception($"No person with identification {idNumber}");
            var identification = await _identificationRepository.GetByIndentificationNumberAsync(idNumber);
            if (identification == null)
                throw exception;
            var data = await _contactRepository.GetByIdAsync(identification.ContactId);
            if (identification == null)
                throw exception;
            return _mapper.Map<ContactViewModel>(data);
        }

        public async Task<bool> ContactExistsByEmailAsync(string data)
        {
            return await _contactRepository.ContactExistsByEmailAsync(data);
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