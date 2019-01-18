using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;
using AutoMapper;

namespace App.Areas.Crm.Utils 
{
    public class CrmMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            // Contact
            config.CreateMap<ContactViewModel, Contact>();

            config.CreateMap<Contact, ContactViewModel>();

            // Identification
            config.CreateMap<IdentificationViewModel, Identification>();
            config.CreateMap<Identification, IdentificationViewModel>();

            // Person
            config.CreateMap<PersonViewModel, Person>();
            config.CreateMap<Person, PersonViewModel>();
    
            // Company
            config.CreateMap<CompanyViewModel, Company>();
            config.CreateMap<Company, CompanyViewModel>();

            // Telephone
            config.CreateMap<PhoneViewModel, Phone>();
            config.CreateMap<Phone, PhoneViewModel>();

            // Email
            config.CreateMap<EmailViewModel, Email>();
            config.CreateMap<Email, EmailViewModel>();

            // Address
            config.CreateMap<AddressViewModel, Address>();
            config.CreateMap<Address, AddressViewModel>();
        }
    }
}
