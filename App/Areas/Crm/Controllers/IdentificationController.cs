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
    /// Identification API
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/identification")]
    public class IdentificationController : BaseController
    {
        private readonly IIdentificationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentificationController> _logger;

        /// <summary>
        /// Controller for identifications
        /// </summary>
        public IdentificationController(IIdentificationRepository repository, IMapper mapper, ILogger<IdentificationController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new identification
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPost("{contactId}")]
        [Produces(typeof(IdentificationViewModel))]
        public async Task<IdentificationViewModel> Post(Guid contactId, [FromBody] IdentificationViewModel model)
        {
            _logger.LogInformation($"add.identification contact: {contactId}");
            AssertValidIds(contactId);
            var data = _mapper.Map<Identification>(model);
            var saved = await _repository.CreateAsync(contactId, data);
            _logger.LogInformation($"added.identification contact: {contactId} record: {saved.Id}");
            return _mapper.Map<IdentificationViewModel>(saved);
        }


        /// <summary>
        /// Updates an identification
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPut("{contactId}")]
        [Produces(typeof(IdentificationViewModel))]
        public async Task<IdentificationViewModel> Put(Guid contactId, [FromBody] IdentificationViewModel model)
        {
            _logger.LogInformation($"update.identification contact: {contactId} record: {model.Id}");
            AssertValidIds(contactId, model.Id);
            var data = _mapper.Map<Identification>(model);
            var saved = await _repository.UpdateAsync(contactId, data);
            _logger.LogInformation($"updated.identification contact: {contactId} record: {saved.Id}");
            return _mapper.Map<IdentificationViewModel>(saved);
        }

        /// <summary>
        /// Deletes an identification
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task<object> Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.identification contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(contactId, id);
            _logger.LogInformation($"deleted.identification contact: {id} record: {id} resp: {resp}");
            return new
            {
                Message = "Deleted Identification"
            };
        }
    }
}