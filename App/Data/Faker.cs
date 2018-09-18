using System;
using System.Collections.Generic;
using App.Areas.Crm.Enums;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.ViewModels;
using Bogus;

namespace App.Data
{
    public class Faker
    {
        public static List<NewPersonViewModel> FakeContacts(int count= 100)
        {
            Randomizer.Seed = new Random(8675309);

            var persons = new Faker<NewPersonViewModel>()
                .RuleFor(u => u.Category, ContactCategory.Person)
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.OtherNames, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.CivilStatus, f => f.PickRandom<CivilStatus>())
                .RuleFor(u => u.Salutation, f => f.PickRandom<Salutation>())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past())
                .RuleFor(u => u.About, f => f.Lorem.Paragraph())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Email, (f,u) => f.Internet.Email(u.FirstName,u.OtherNames))
                .RuleFor(u => u.Tags, f => f.Lorem.Words(2))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber());
            return persons.Generate(count);
        }

        public static List<EventViewModel> FakeEvents(int count = 100)
        {
            Randomizer.Seed = new Random(8675309);

            var persons = new Faker<EventViewModel>()
                .RuleFor(u => u.Name, f => f.Company.CatchPhrase())
                .RuleFor(u => u.StartDate, f => f.Date.Soon(2))
                .RuleFor(u => u.EndDate, f => f.Date.Soon(3))
                .RuleFor(u => u.Venue, f => f.Address.BuildingNumber())
                .RuleFor(u => u.FreeFormAddress, f => f.Address.FullAddress())
                .RuleFor(u => u.Description, f => f.Lorem.Paragraph())
                .RuleFor(u => u.GeoCoOrdinates, f => f.Address.ZipCode())
                .RuleFor(u => u.CreatorId, Guid.NewGuid())
                .RuleFor(u => u.Image, f => f.Internet.Avatar())
                .RuleFor(u => u.Tags, f => f.Lorem.Words(2));
            return persons.Generate(count);
        }
    }
}
