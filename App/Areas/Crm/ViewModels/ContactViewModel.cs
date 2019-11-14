using System;
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
        public IdentificationViewModel[] Identifications { get; set; }
        public PhoneViewModel[] Phones { get; set; }
        public EmailViewModel[] Emails { get; set; }
        public AddressViewModel[] Addresses { get; set; }

        public OccasionViewModel[] Occasions { get; set; }
        public string[] Tags { get; set; }

        public MetaData MetaData { get; set; }
    }

    public class MinimalContact
    {
        public Guid Id { get; set; }
        public Salutation? Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{Salutation} {FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
    }
}