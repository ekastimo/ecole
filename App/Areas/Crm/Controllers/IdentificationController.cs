using System;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Identification;
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
    /// Identification API
    /// </summary>
    [Authorize]
    [AreaName("Crm")]
    [Route("api/crm/identification")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class IdentificationController : Controller
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

        private void AssertValidIds(params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                if (guid == Guid.Empty)
                    throw new ClientFriendlyException($"Invalid record id:{guid}");
            }
        }

        /// <summary>
        /// Create a new identification
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(IdentificationViewModel))]
        public async Task<IdentificationViewModel> Post([FromBody] IdentificationViewModel model)
        {
            _logger.LogInformation($"add.identification contact: {model.ContactId}");
            AssertValidIds(model.ContactId);
            var data = _mapper.Map<Identification>(model);
            var saved = await _repository.CreateAsync(data);
            _logger.LogInformation($"added.identification contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<IdentificationViewModel>(saved);
        }


        /// <summary>
        /// Updates an identification
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(IdentificationViewModel))]
        public async Task<IdentificationViewModel> Put([FromBody] IdentificationViewModel model)
        {
            _logger.LogInformation($"update.identification contact: {model.ContactId} record: {model.Id}");
            AssertValidIds(model.ContactId, model.Id);
            var data = _mapper.Map<Identification>(model);
            var saved = await _repository.UpdateAsync(data);
            _logger.LogInformation($"updated.identification contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<IdentificationViewModel>(saved);
        }

        /// <summary>
        /// Deletes an identification
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.identification contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.identification contact: contact: {id} record: {id} resp: {resp}");
        }
    }
}