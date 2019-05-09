using System;
using System.Collections.Generic;
using App.Areas.Crm.Enums;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.ViewModels;
using Bogus;
using System.Linq;
using App.Areas.Chc.ViewModel;

namespace App.Data
{
    public class FakeData
    {
        public static string[] EventTags = {"Party", "food", "Money", "Kadanke", "Free", "Family", "educational"};
        public static string[] ContactTags = {"Client", "Service Provider", "Customer", "Afro"};

        static FakeData()
        {
            Randomizer.Seed = new Random(8675309);
        }

        public static List<LocationViewModel> FakeLocations()
        {
          
            return new List<LocationViewModel>
            {
                new LocationViewModel
                {
                    Id = "WHKatiKati",
                    Name = "WHKatiKati",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHNalya",
                    Name = "WHNalya",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHJinja",
                    Name = "WHJinja",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHGayaza",
                    Name = "WHGayaza",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHBugolobi",
                    Name = "WHBugolobi",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHEntebe",
                    Name = "WHEntebe",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHMukono",
                    Name = "WHMukono",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHKubuye",
                    Name = "WHKubuye",
                    Details = "",
                    MeetingTimes = new List<string>{ "9am", "11am" },
                },
                new LocationViewModel
                {
                    Id = "WHMakerere",
                    Name = "WHMakerere",
                    Details = "",
                    MeetingTimes = new List<string>{ "10am" },
                },
                new LocationViewModel
                {
                    Id = "WHMbarara",
                    Name = "WHMbarara",
                    Details = "",
                    MeetingTimes = new List<string>{"10am" },
                },
                new LocationViewModel
                {
                    Id = "WHDowntown",
                    Name = "WHDowntown",
                    Details = "",
                    MeetingTimes = new List<string>{"10am" },
                }
            };
        }

        public static List<CellGroupViewModel> FakeCellGroups(int count = 100)
        {

            return new List<CellGroupViewModel>
            {
                new CellGroupViewModel
                {
                    Location = "WHKatiKati",
                    Id = "DUNAMIS",
                    Name = "DUNAMIS",
                    Details = "DUNAMIS",
                    MeetingTimes =  new List<string>{"6pm"},
                },
                new CellGroupViewModel
                {
                    Location = "WHKatiKati",
                    Id = "KMC",
                    Name = "KMC",
                    Details = "KMC",
                    MeetingTimes =  new List<string>{"6pm"},
                }
            };
        }

        public static List<NewPersonViewModel> FakeContacts(int count = 100)
        {
            var persons = new Faker<NewPersonViewModel>()
                .RuleFor(u => u.ChurchLocation, "WHKatiKati")
                .RuleFor(u => u.CellGroup, "DUNAMIS")
                .RuleFor(u => u.Category, ContactCategory.Person)
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.MiddleName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.CivilStatus, f => f.PickRandom<CivilStatus>())
                .RuleFor(u => u.Salutation, f => f.PickRandom<Salutation>())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past())
                .RuleFor(u => u.About, f => f.Lorem.Paragraph())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Tags, f => f.PickRandom(ContactTags.Select(it => it.ToLower()), 3).ToArray())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("####-###-###"));
            return persons.Generate(count);
        }

        public static List<ItemViewModel> FakeEventItems(Guid contactId, Guid eventId, int count = 100)
        {
            var persons = new Faker<ItemViewModel>()
                .RuleFor(u => u.Name, f => f.Company.CatchPhrase())
                .RuleFor(u => u.StartDate, f => f.Date.Soon(2))
                .RuleFor(u => u.EndDate, f => f.Date.Soon(3))
                .RuleFor(u => u.Details, f => f.Lorem.Paragraph())
                .RuleFor(u => u.CreatedBy, contactId)
                .RuleFor(u => u.EventId, eventId)
                .RuleFor(u => u.Images, f => Enumerable.Range(1, 3).Select(it => f.Image.LoremPixelUrl()).ToArray());
            return persons.Generate(count);
        }

        public static List<EventViewModel> FakeEvents(Guid contactId, int count = 100)
        {
            var persons = new Faker<EventViewModel>()
                .RuleFor(u => u.Name, f => f.Company.CatchPhrase())
                .RuleFor(u => u.StartDate, f => f.Date.Soon(2))
                .RuleFor(u => u.EndDate, f => f.Date.Soon(3))
                .RuleFor(u => u.Venue, f => f.Address.BuildingNumber())
                .RuleFor(u => u.FreeFormAddress, f => f.Address.FullAddress())
                .RuleFor(u => u.Details, f => f.Lorem.Paragraph())
                .RuleFor(u => u.GeoCoOrdinates, f => f.Address.ZipCode())
                .RuleFor(u => u.CreatedBy, contactId)
                .RuleFor(u => u.Tags, f => f.PickRandom(EventTags.Select(it => it.ToLower()), 3).ToArray())
                .RuleFor(u => u.Images, f => Enumerable.Range(1, 3).Select(it => f.Image.LoremPixelUrl()).ToArray());
            return persons.Generate(count);
        }
    }
}