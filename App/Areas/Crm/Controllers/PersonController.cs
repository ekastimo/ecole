using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Chc.Repositories;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using App.Areas.Doc.Services;
using App.Areas.Doc.ViewModels;
using Core.Controllers;
using Core.Exceptions;
using Core.Extensions;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Crm.Controllers
{
    [AreaName("Crm")]
    [Route("api/crm/person")]
    public class PersonController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;
        private readonly IDocService _docService;
        private readonly ILocationRepository _locationRepository;
        private readonly ICellGroupRepository _cellGroupRepository; 
     
        public PersonController(IHttpContextAccessor httpContextAccessor, IContactService contactService, ILogger<ContactController> logger,
            IDocService docService, ILocationRepository locationRepository, ICellGroupRepository cellGroupRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _contactService = contactService;
            _logger = logger;
            _docService = docService;
            _locationRepository = locationRepository;
            _cellGroupRepository = cellGroupRepository;
        }


        /// <summary>
        /// Searches people
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<MinimalContact>))]
        public async Task<List<MinimalContact>> SearchContacts(SearchBase request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"search.contacts ${json}");
            var data = (await _contactService.SearchMinimalAsync(request)).ToList();
            _logger.LogInformation($"found.contacts {data.Count}");
            return data;
        }

        /// <summary>
        /// Update basic information
        /// </summary>
        /// <param name="contactId">The contact to be updated</param>
        /// <param name="person">Person data</param>
        /// <returns></returns>
        [HttpPut("{contactId}")]
        public async Task<ContactViewModel> UpdatePerson(Guid contactId, [FromBody] PersonViewModel person)
        {

            if (contactId == Guid.Empty)
            {
                throw new ClientFriendlyException($"Invalid contact id: {contactId}");
            }

            var contact = await _contactService.UpdatePerson(contactId, person);
            if (contact == null)
            {
                throw new ClientFriendlyException($"Invalid contact id: {contactId}");
            }
            return contact;
        }


        /// <summary>
        /// Creates a new person
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ContactViewModel))]
        public async Task<ContactViewModel> Create([FromBody] ContactViewModel model)
        {
            // todo Add Validation
            _logger.LogInformation("add.person");
            var data = await _contactService.CreateAsync(model);
            _logger.LogInformation($"added.person {data.Id}");
            return data;
        }


        /// <summary>
        /// Update avatar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("avatar")]
        public async Task<ActionResult> UpdateAvatar(UploadRequest request)
        {
            var contactId = request.RefrenceId;
            if (contactId == null || contactId.Value == Guid.Empty)
            {
                throw new ClientFriendlyException($"Invalid contact id: {contactId}");
            }

            var contact = await _contactService.GetByIdAsync(contactId.Value);
            if (contact == null)
            {
                throw new ClientFriendlyException($"Invalid contact id: {contactId}");
            }

            var resp = await _docService.Upload(request);
            var avatar = $"http://localhost:9001/api/docs/download/{resp.Id}";
            contact.Person.Avatar = avatar;
            await _contactService.UpdateAsync(contact);
            return Ok(new { message = "Upload Successful", avatar });
        }
    }
}
