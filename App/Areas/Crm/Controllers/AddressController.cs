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
    /// Address API
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/address")]
    public class AddressController : BaseController
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressController> _logger;

        /// <summary>
        /// Controller for addresss
        /// </summary>
        public AddressController(IAddressRepository repository, IMapper mapper, ILogger<AddressController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new address
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPost("{contactId}")]
        [Produces(typeof(AddressViewModel))]
        public async Task<AddressViewModel> Post(Guid contactId, [FromBody] AddressViewModel model)
        {
            _logger.LogInformation($"add.address contact: {contactId}");
            AssertValidIds(contactId);
            var data = _mapper.Map<Address>(model);
            var saved = await _repository.CreateAsync(contactId, data);
            _logger.LogInformation($"added.address contact: {contactId} record: {saved.Id}");
            return _mapper.Map<AddressViewModel>(saved);
        }


        /// <summary>
        /// Updates an address
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPut("{contactId}")]
        [Produces(typeof(AddressViewModel))]
        public async Task<AddressViewModel> Put(Guid contactId, [FromBody] AddressViewModel model)
        {
            _logger.LogInformation($"update.address contact: {contactId} record: {model.Id}");
            AssertValidIds(contactId, model.Id);
            var data = _mapper.Map<Address>(model);
            var saved = await _repository.UpdateAsync(contactId, data);
            _logger.LogInformation($"updated.address contact: {contactId} record: {saved.Id}");
            return _mapper.Map<AddressViewModel>(saved);
        }

        /// <summary>
        /// Deletes an address
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.address contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(contactId, id);
            _logger.LogInformation($"deleted.address contact: {id} record: {id} resp: {resp}");
        }
    }
}