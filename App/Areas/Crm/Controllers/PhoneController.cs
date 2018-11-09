using System;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Controllers;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Crm.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Phone API
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/phone")]
    public class PhoneController : BaseController
    {
        private readonly IPhoneRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PhoneController> _logger;

        /// <summary>
        /// Controller for phones
        /// </summary>
        public PhoneController(IPhoneRepository repository, IMapper mapper, ILogger<PhoneController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new phone
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPost("{contactId}")]
        [Produces(typeof(PhoneViewModel))]
        public async Task<PhoneViewModel> Post(Guid contactId, [FromBody] PhoneViewModel model)
        {
            _logger.LogInformation($"add.phone contact: {contactId}");
            AssertValidIds(contactId);
            var data = _mapper.Map<Phone>(model);
            var saved = await _repository.CreateAsync(contactId, data);
            _logger.LogInformation($"added.phone contact: {contactId} record: {saved.Id}");
            return _mapper.Map<PhoneViewModel>(saved);
        }


        /// <summary>
        /// Updates an phone
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPut("{contactId}")]
        [Produces(typeof(PhoneViewModel))]
        public async Task<PhoneViewModel> Put(Guid contactId, [FromBody] PhoneViewModel model)
        {
            _logger.LogInformation($"update.phone contact: {contactId} record: {model.Id}");
            AssertValidIds(contactId, model.Id);
            var data = _mapper.Map<Phone>(model);
            var saved = await _repository.UpdateAsync(contactId, data);
            _logger.LogInformation($"updated.phone contact: {contactId} record: {saved.Id}");
            return _mapper.Map<PhoneViewModel>(saved);
        }

        /// <summary>
        /// Deletes an phone
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.phone contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(contactId, id);
            _logger.LogInformation($"deleted.phone contact: {id} record: {id} resp: {resp}");
        }
    }
}