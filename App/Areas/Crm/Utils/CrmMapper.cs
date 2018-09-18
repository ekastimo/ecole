#pragma warning disable CS1591 // Missing XML comment
using System.Collections.Generic;
using System.Linq;
using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.Models;
using AutoMapper;

namespace App.Areas.Crm.Utils 
{
    public class CrmMapper
    {

        private static ICollection<ContactCTag> ParseTags(string[] tags)
        {
            var hasTags = tags?.Any() ?? false;
            if (!hasTags)
                return null;
            return tags.Select(it => new ContactCTag
            {
                Tag = new CTag { Id = it }
            }).ToList();
        }

        private static string[] ParseCTags(ICollection<ContactCTag> ctags)
        {
            var hasTags = ctags?.Any() ?? false;
            if (!hasTags)
                return null;
            return ctags.Select(it => it.TagId).ToArray();
        }


        public static void MapModels(IMapperConfigurationExpression config)
        {
            // Contact
            config.CreateMap<ContactViewModel, Contact>()
                .ForMember(d=>d.ContactTags,o=>o.MapFrom(s=> ParseTags(s.Tags)));

            config.CreateMap<Contact, ContactViewModel>()
                .ForMember(d=>d.Tags,o=>o.MapFrom(s=>ParseCTags(s.ContactTags)));

            // Identification
            config.CreateMap<IdentificationViewModel, Identification>();
            config.CreateMap<Identification, IdentificationViewModel>();

            // Person
            config.CreateMap<PersonViewModel, Person>();
            config.CreateMap<Person, PersonViewModel>();

            // Person
            config.CreateMap<MinimalContact, Person>();
            config.CreateMap<Person, MinimalContact>();

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
