using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Chc.Repositories;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Doc.Services;
using Core.Controllers;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<ContactController> _logger;
        private readonly ILocationRepository _locationRepository;
        private readonly ICellGroupRepository _cellGroupRepository;

        /// <inheritdoc />
        public ContactController(IHttpContextAccessor httpContextAccessor, IContactService contactService, ILogger<ContactController> logger,
            IDocService docService,ILocationRepository locationRepository,ICellGroupRepository cellGroupRepository)
        {
            _contactService = contactService;
            _logger = logger;
            _locationRepository = locationRepository;
            _cellGroupRepository = cellGroupRepository;
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
        [Produces(typeof(IEnumerable<ContactViewModel>))]
        public async Task<List<ContactViewModel>> Get(ContactSearchRequest request)
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
        /// <param name="guidList"></param>
        /// <returns></returns>
        [HttpPost("ids")]
        [Produces(typeof(IEnumerable<ContactViewModel>))]
        public async Task<List<ContactViewModel>> FindByIds([FromBody] List<Guid> guidList)
        {
            var json = JsonConvert.SerializeObject(guidList);
            _logger.LogInformation($"search.contacts.ids ${json}");
            var data = (await _contactService.GetContactsAsync(guidList)).ToList();
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
    }
}