using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Events.Controllers;
using Core.Controllers;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Crm.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// All end-points dealing with contacts 
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/contact")]
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        private readonly ILogger<EventController> _logger;

        /// <inheritdoc />
        public ContactController(IContactService contactService, ILogger<EventController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }


        /// <summary>
        /// Creates Contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Create([FromBody] ContactViewModel model)
        {
            _logger.LogInformation("add.contact");
            var data = await _contactService.CreateAsync(model);
            _logger.LogInformation($"added.person {data.Id}");
            return data;
        }
        

        /// <summary>
        /// Searches contacts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<MinimalContact>))]
        public async Task<IEnumerable<MinimalContact>> Search(ContactSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"search.contacts ${json}");
            var data = (await _contactService.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.contacts {data.Count}");
            return data;
        }

        /// <summary>
        /// Gets a specific contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Get(Guid id)
        {
            _logger.LogInformation($"search.contact.id ${id}");
            var data = await _contactService.GetByIdAsync(id);
            if (data == null)
                throw new NotFoundException($"Invalid record id:{id}");
            _logger.LogInformation($"found.contact {data.Id}");
            return data;
        }

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Update([FromBody] ContactViewModel model)
        {
            // TODO Do some validation
            _logger.LogInformation($"update.contact ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var data = await _contactService.UpdateAsync(model);
            _logger.LogInformation($"updated.contact ${data.Id}");
            return data;
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.contact ${id}");
            Tk.AssertValidIds(id);
            var data = await _contactService.DeleteAsync(id);
            _logger.LogInformation($"delete.contact ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }

        /// <summary>
        /// Returns contacts with all the listed ids
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        [HttpPost("byids")]
        [Produces(typeof(IEnumerable<MinimalContact>))]
        public async Task<IEnumerable<MinimalContact>> FindByIds([FromBody] List<Guid> guids)
        {
            var json = JsonConvert.SerializeObject(guids);
            _logger.LogInformation($"search.contacts.ids ${json}");
            var data = (await _contactService.GetContactsAsync(guids)).ToList();
            _logger.LogInformation($"found.contacts {data.Count}");
            return data;
        }

      

        /// <summary>
        /// Gets a specific contact by identification number
        /// </summary>
        /// <param name="idNumber">Contact identification</param>
        /// <returns></returns>
        [HttpGet("identification/{idNumber}")]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> GetByIdentification(string idNumber)
        {
            _logger.LogInformation($"search.contact.nin ${idNumber}");
            var data = await _contactService.GetByIdentificationAsync(idNumber);
            if (data == null)
                throw new NotFoundException($"Invalid record nin:{idNumber}");
            _logger.LogInformation($"found.contact {data.Id}");
            return data;
        }


        /// <summary>
        /// Creates a new person
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("person")]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Create([FromBody] NewPersonViewModel model)
        {
            _logger.LogInformation("add.person");
            var data = await _contactService.CreateAsync(model);
            _logger.LogInformation($"added.person {data.Id}");
            return data;
        }

        /// <summary>
        /// Creates a new company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("company")]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Create([FromBody] NewCompanyViewModel model)
        {
            _logger.LogInformation("add.comapny");
            var data = await _contactService.CreateAsync(model);
            _logger.LogInformation($"added.comapny {data.Id}");
            return data;
        }

    }
}