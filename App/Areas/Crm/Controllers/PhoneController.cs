using System;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Phone;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Crm.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Phone API
    /// </summary>
    [Authorize]
    [AreaName("Crm")]
    [Route("api/crm/phone")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class PhoneController : Controller
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

        private void AssertValidIds(params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                if (guid == Guid.Empty)
                    throw new ClientFriendlyException($"Invalid record id:{guid}");
            }
        }

        /// <summary>
        /// Create a new phone
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(PhoneViewModel))]
        public async Task<PhoneViewModel> Post([FromBody] PhoneViewModel model)
        {
            _logger.LogInformation($"add.phone contact: {model.ContactId}");
            AssertValidIds(model.ContactId);
            var data = _mapper.Map<Phone>(model);
            var saved = await _repository.CreateAsync(data);
            _logger.LogInformation($"added.phone contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<PhoneViewModel>(saved);
        }


        /// <summary>
        /// Updates an phone
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(PhoneViewModel))]
        public async Task<PhoneViewModel> Put([FromBody] PhoneViewModel model)
        {
            _logger.LogInformation($"update.phone contact: {model.ContactId} record: {model.Id}");
            AssertValidIds(model.ContactId, model.Id);
            var data = _mapper.Map<Phone>(model);
            var saved = await _repository.UpdateAsync(data);
            _logger.LogInformation($"updated.phone contact: {model.ContactId} record: {saved.Id}");
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
            var resp = await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.phone contact: contact: {id} record: {id} resp: {resp}");
        }
    }
}