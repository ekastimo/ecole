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
    public class ChcTest
    {
        private ContactRepository ContactRepository { get; }
        private EmailRepository EmailRepository { get; }
        private ContactService ContactService { get; }
        public IdentificationRepository IdentificationRepository { get; }
        private readonly ILogger<EventService> _eventLogger = Mock.Of<ILogger<EventService>>();
        private ILogger<ItemService> _eventItemLogger = Mock.Of<ILogger<ItemService>>();
        private readonly ILogger<ContactService> _contactLogger = Mock.Of<ILogger<ContactService>>();
        private Mapper Mapper { get; }

        public ChcTest()
        {
            
        }

        [Fact]
        public async Task CreateContact()
        {
            var location = FakeData.FakeLocations(1).First();
            Console.WriteLine(location);
        }

   
    }
}