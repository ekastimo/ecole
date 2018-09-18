using System;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Address;
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
    /// Address API
    /// </summary>
    [Authorize]
    [AreaName("Crm")]
    [Route("api/crm/address")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class AddressController : Controller
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressController> _logger;

        /// <summary>
        /// Controller for addresses
        /// </summary>
        public AddressController(IAddressRepository repository, IMapper mapper, ILogger<AddressController> logger)
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
        /// Create a new address
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(AddressViewModel))]
        public async Task<AddressViewModel> Post([FromBody] AddressViewModel model)
        {
            _logger.LogInformation($"add.address contact: {model.ContactId}");
            AssertValidIds(model.ContactId);
            var data = _mapper.Map<Address>(model);
            var saved = await _repository.CreateAsync(data);
            _logger.LogInformation($"added.address contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<AddressViewModel>(saved);
        }


        /// <summary>
        /// Updates an address
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(AddressViewModel))]
        public async Task<AddressViewModel> Put([FromBody] AddressViewModel model)
        {
            _logger.LogInformation($"update.address contact: {model.ContactId} record: {model.Id}");
            AssertValidIds(model.ContactId, model.Id);
            var data = _mapper.Map<Address>(model);
            var saved = await _repository.UpdateAsync(data);
            _logger.LogInformation($"updated.address contact: {model.ContactId} record: {saved.Id}");
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
            var resp = await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.address contact: contact: {id} record: {id} resp: {resp}");
        }
    }
}