using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.Repositories.Event;
using App.Areas.Events.Repositories.Item;
using App.Areas.Events.Services.Event;
using App.Areas.Events.Services.Item;
using App.Data;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Test
{
    public class FakerTest
    {
        public EventRepository EventRepository { get; }
        public ItemRepository EventItemRepository { get; }
        public ContactRepository ContactRepository { get; }
        public IdentificationRepository IdentificationRepository { get; }
   

        public Mapper Mapper { get; }

        public FakerTest()
        {
           
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new ApplicationDbContext(config);
            EventRepository = new EventRepository(dbContext);
            EventItemRepository = new ItemRepository(dbContext);
            ContactRepository = new ContactRepository(dbContext);
            IdentificationRepository = new IdentificationRepository(dbContext);


            var mapperConfig = new MapperConfiguration(CustomMapper.CreateConfigs);
            Mapper = new Mapper(mapperConfig);
        }
    
        [Fact]
        public async Task SeedDatabase()
        {
            var eventLogger = Mock.Of<ILogger<EventService>>();
            var eventItemLogger = Mock.Of<ILogger<ItemService>>();
            var contactLogger = Mock.Of<ILogger<ContactService>>();
            var contactService = new ContactService(ContactRepository, IdentificationRepository, Mapper, contactLogger);

           
       

            var fakeContacts = FakeData.FakeContacts();
            foreach (var fakeContact in fakeContacts)
            {
                await contactService.CreateAsync(fakeContact);
            }

            
            var contacts = await contactService.SearchAsync(new ContactSearchRequest());
            var creatorId = contacts.First().Id;
            var data = FakeData.FakeEvents(creatorId);
            var eventService = new EventService(EventRepository, Mapper, eventLogger);
            var eventItemService = new ItemService(EventItemRepository, Mapper, eventItemLogger);
            foreach (var model in data)
            {
                var saved = await eventService.CreateAsync(model);
                var items = FakeData.FakeEventItems(creatorId, saved.Id,12);
                foreach (var item in items)
                {
                    await eventItemService.CreateAsync(item);
                }
            }
        }
    }
}