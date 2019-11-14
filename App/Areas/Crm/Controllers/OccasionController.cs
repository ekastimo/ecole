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
    /// Occasion API
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/occasion")]
    public class OccasionController : BaseController
    {
        private readonly IOccasionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<OccasionController> _logger;

        /// <summary>
        /// Controller for occasions
        /// </summary>
        public OccasionController(IOccasionRepository repository, IMapper mapper, ILogger<OccasionController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new occasion
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPost("{contactId}")]
        [Produces(typeof(OccasionViewModel))]
        public async Task<OccasionViewModel> Post(Guid contactId, [FromBody] OccasionViewModel model)
        {
            _logger.LogInformation($"add.occasion contact: {contactId}");
            AssertValidIds(contactId);
            var data = _mapper.Map<Occasion>(model);
            var saved = await _repository.CreateAsync(contactId, data);
            _logger.LogInformation($"added.occasion contact: {contactId} record: {saved.Id}");
            return _mapper.Map<OccasionViewModel>(saved);
        }


        /// <summary>
        /// Updates an occasion
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPut("{contactId}")]
        [Produces(typeof(OccasionViewModel))]
        public async Task<OccasionViewModel> Put(Guid contactId, [FromBody] OccasionViewModel model)
        {
            _logger.LogInformation($"update.occasion contact: {contactId} record: {model.Id}");
            AssertValidIds(contactId, model.Id);
            var data = _mapper.Map<Occasion>(model);
            var saved = await _repository.UpdateAsync(contactId, data);
            _logger.LogInformation($"updated.occasion contact: {contactId} record: {saved.Id}");
            return _mapper.Map<OccasionViewModel>(saved);
        }

        /// <summary>
        /// Deletes an occasion
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task<object> Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.occasion contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(contactId, id);
            _logger.LogInformation($"deleted.occasion contact: {id} record: {id} resp: {resp}");
            return new
            {
                Message = "Deleted Occasion"
            };
        }
    }
}