using System;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Services;
using App.Areas.Events.Services.Event;
using App.Areas.Events.Services.Item;
using App.Data;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Test.Chc
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