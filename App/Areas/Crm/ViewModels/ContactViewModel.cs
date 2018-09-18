using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class ContactViewModel : ViewModel
    {
        public ContactCategory Category { get; set; }
        public PersonViewModel Person { get; set; }
        public CompanyViewModel Company { get; set; }
        public ICollection<IdentificationViewModel> Identifications { get; set; }
        public ICollection<PhoneViewModel> Phones { get; set; }
        public ICollection<EmailViewModel> Emails { get; set; }
        public ICollection<AddressViewModel> Addresses { get; set; }
        public string[] Tags { get; set; }
    }

    public class NewContactViewModel
    {
        [Required] public ContactCategory Category { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Phone { get; set; }
        public IdentificationCategory? IdentificationCategory { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? IdentificationValidFrom { get; set; }
        public DateTime? IdentificationValidTo { get; set; }
        public ICollection<string> Tags { get; set; }
    }

    public class NewPersonViewModel : NewContactViewModel
    {
        public new ContactCategory Category { get; set; } = ContactCategory.Person;
        [Required] public string FirstName { get; set; }
        [Required] public string OtherNames { get; set; }
        [Required] public Gender Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        public CivilStatus? CivilStatus { get; set; }
        public Salutation? Salutation { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
    }

    public class NewCompanyViewModel : NewContactViewModel
    {
        public new ContactCategory Category { get; set; } = ContactCategory.Company;
        [Required] public CompanyCategory CompanyCategory { get; set; }
        [Required] public string Name { get; set; }
    }

    public class MinimalContact
    {
        public Guid Id { get; set; }
        public ContactCategory Category { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Name { get; set; }
        public string FullName => Category == ContactCategory.Person ? $"{FirstName} {OtherNames}" : Name;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public CivilStatus? CivilStatus { get; set; }
        public Salutation? Salutation { get; set; }
    }
}