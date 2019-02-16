using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Services;
using App.Areas.Events.Services.Event;
using App.Areas.Events.Services.Item;
using App.Data;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Moq;
using Xunit;

namespace App.Test.Crm
{
    public class ContactTest
    {
        private ContactRepository ContactRepository { get; }
        private EmailRepository EmailRepository { get; }
        private ContactService ContactService { get; }
        public IdentificationRepository IdentificationRepository { get; }
        private readonly ILogger<EventService> _eventLogger = Mock.Of<ILogger<EventService>>();
        private ILogger<ItemService> _eventItemLogger = Mock.Of<ILogger<ItemService>>();
        private readonly ILogger<ContactService> _contactLogger = Mock.Of<ILogger<ContactService>>();
        private Mapper Mapper { get; }

        public ContactTest()
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new ApplicationDbContext(config,null);
            ContactRepository = new ContactRepository(dbContext);
            EmailRepository = new EmailRepository(dbContext);
            IdentificationRepository = new IdentificationRepository(dbContext);
            var mapperConfig = new MapperConfiguration(CustomMapper.CreateConfigs);
            Mapper = new Mapper(mapperConfig);
            ContactService = new ContactService(ContactRepository, Mapper, _contactLogger);
        }

        [Fact]
        public async Task CreateContact()
        {
            var contact = FakeData.FakeContacts(1).First();
            var data = await ContactService.CreateAsync(contact);
            Console.WriteLine(data);
        }

        [Fact]
        public async Task AddEmail()
        {
            var contact = FakeData.FakeContacts(2)[1];
            var data = await ContactService.CreateAsync(contact);
            var contactId = data.Id;
            var email = new Email
            {
                Address = "test@email.com",
                Category = EmailCategory.Personal,
                IsPrimary = false
            };
            var emailSaved = await EmailRepository.CreateAsync(contactId, email);
            emailSaved.Address = "test@email.bar";
            var emailUpdated = await EmailRepository.UpdateAsync(contactId, emailSaved);
            var deletion = await EmailRepository.DeleteAsync(contactId, emailUpdated.Id);
            Debug.WriteLine(deletion);
        }
    }
}