using System;
using System.Collections.Generic;
using App.Areas.Crm.Enums;
using App.Areas.Events.ViewModels;
using Bogus;
using System.Linq;
using App.Areas.Chc.ViewModel;
using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;

namespace App.Data
{
    public class FakeData
    {
        public static string[] EventTags = {"Party", "food", "Money", "Kadanke", "Free", "Family", "educational"};
        public static string[] ContactTags = {"Client", "Customer", "Afro"};

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
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHNalya",
                    Name = "WHNalya",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHJinja",
                    Name = "WHJinja",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHGayaza",
                    Name = "WHGayaza",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHBugolobi",
                    Name = "WHBugolobi",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHEntebe",
                    Name = "WHEntebe",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHMukono",
                    Name = "WHMukono",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHKubuye",
                    Name = "WHKubuye",
                    Details = "",
                    MeetingTimes = new List<string> {"9am", "11am"},
                },
                new LocationViewModel
                {
                    Id = "WHMakerere",
                    Name = "WHMakerere",
                    Details = "",
                    MeetingTimes = new List<string> {"10am"},
                },
                new LocationViewModel
                {
                    Id = "WHMbarara",
                    Name = "WHMbarara",
                    Details = "",
                    MeetingTimes = new List<string> {"10am"},
                },
                new LocationViewModel
                {
                    Id = "WHDowntown",
                    Name = "WHDowntown",
                    Details = "",
                    MeetingTimes = new List<string> {"10am"},
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
                    MeetingTimes = new List<string> {"6pm"},
                },
                new CellGroupViewModel
                {
                    Location = "WHKatiKati",
                    Id = "KMC",
                    Name = "KMC",
                    Details = "KMC",
                    MeetingTimes = new List<string> {"6pm"},
                }
            };
        }

        private static T[] ToArray<T>(params T[] data)
        {
            return data;
        }

        public static List<ContactViewModel> FakeContacts(int count = 100)
        {
            var persons = new Faker<ContactViewModel>()
                .RuleFor(u => u.MetaData, f => new MetaData
                {
                    ChurchLocation = "WHKatiKati",
                    CellGroup = "DUNAMIS"
                })
                .RuleFor(u => u.Category, ContactCategory.Person)
                .RuleFor(u => u.Person, f => new PersonViewModel
                {
                    FirstName = f.Name.FirstName(),
                    LastName = f.Name.LastName(),
                    MiddleName = f.Name.LastName(),
                    Gender = f.PickRandom<Gender>(),
                    CivilStatus = f.PickRandom<CivilStatus>(),
                    Salutation = f.PickRandom<Salutation>(),
                    About = f.Lorem.Paragraph(),
                    Avatar = f.Internet.Avatar()
                })
                .RuleFor(u => u.Events, f => ToArray(
                    new ContactEventViewModel
                    {
                        Category = ContactEventCategory.Birthday,
                        Value = f.Date.Past()
                    }))
                .RuleFor(u => u.Emails, (f, u) => ToArray(
                    new EmailViewModel
                    {
                        Category = EmailCategory.Personal,
                        Value = f.Internet.Email(u.Person.FirstName, u.Person.LastName)
                    }))
                .RuleFor(u => u.Phones, (f, u) => ToArray(
                    new PhoneViewModel
                    {
                        Category = PhoneCategory.Mobile,
                        Value = f.Phone.PhoneNumber("07########")
                    }))
                .RuleFor(u => u.Tags, f => f.PickRandom(ContactTags.Select(it => it.ToLower()), 3).ToArray());

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