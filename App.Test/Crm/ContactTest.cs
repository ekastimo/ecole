using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.Services.Event;
using App.Data;
using AutoMapper;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace App.Test.Crm
{
    public class ContactTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private ContactRepository ContactRepository { get; }
        private EmailRepository EmailRepository { get; }
        private ContactService ContactService { get; }
        public IdentificationRepository IdentificationRepository { get; }
        private readonly ILogger<EventService> _eventLogger = Mock.Of<ILogger<EventService>>();
        private readonly ILogger<ApplicationDbContext> _logger = Mock.Of<ILogger<ApplicationDbContext>>();
        private readonly ILogger<ContactService> _contactLogger = Mock.Of<ILogger<ContactService>>();
        private Mapper Mapper { get; }
        readonly MongoDbRunner _runner = MongoDbRunner.Start(singleNodeReplSet: true);

        private readonly NewPersonViewModel _contact = new NewPersonViewModel
        {
            FirstName = "Timothy",
            LastName = "Kasasa",
            MiddleName = "Emmanuel",
            AgeRange = "18-35",
            About = "About Me",
            Avatar = "",
            Gender = Gender.Male,
            DateOfBirth = DateTime.Today,
            CellGroup = "KMC",
            ChurchLocation = "WHKatiKati",
            Phone = "0772120258",
            Email = "ekastimo@gmail.com"
        };

        private Guid _guid;

        public ContactTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MongoConnection:ConnectionString", _runner.ConnectionString),
                    new KeyValuePair<string, string>("MongoConnection:Database", "test")
                })
                .Build();
            var dbContext = new ApplicationDbContext(config, _logger);
            ContactRepository = new ContactRepository(dbContext);
            EmailRepository = new EmailRepository(dbContext);
            IdentificationRepository = new IdentificationRepository(dbContext);
            var mapperConfig = new MapperConfiguration(CustomMapper.CreateConfigs);
            Mapper = new Mapper(mapperConfig);
            ContactService = new ContactService(ContactRepository, Mapper, _contactLogger);
        }

        [Fact]
        public async Task CanCreateContact()
        {
            var data = await ContactService.CreateAsync(_contact);
            Assert.NotEqual(Guid.Empty, data.Id);
            _guid = data.Id;
        }

        [Fact]
        public async Task EmailManipulationWorks ()
        {
            _contact.Email = "ekastimo@yahoo.com";
            var data = await ContactService.CreateAsync(_contact);
            Assert.NotEqual(Guid.Empty, data.Id);
           
            var contactId = data.Id;
            var email = new Email
            {
                Value = "test@email.com",
                Category = EmailCategory.Personal,
                IsPrimary = false
            };
            var emailSaved = await EmailRepository.CreateAsync(contactId, email);
            var wtEmail = await ContactService.GetByIdAsync(data.Id);
            Assert.Equal(2, wtEmail.Emails.Length);
            emailSaved.Value = "test@email.bar";
            var emailUpdated = await EmailRepository.UpdateAsync(contactId, emailSaved);
            var wtEmail2 = await ContactService.GetByIdAsync(data.Id);
            Assert.Contains(wtEmail2.Emails, it =>it.Value == emailSaved.Value);
            await EmailRepository.DeleteAsync(contactId, emailUpdated.Id);
            var afterDelete = await ContactService.GetByIdAsync(data.Id);
            Assert.Single(afterDelete.Emails);
            
        }
    }
}