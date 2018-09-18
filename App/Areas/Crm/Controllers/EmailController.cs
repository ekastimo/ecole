using System;
using System.Threading.Tasks;
using App.Areas.Crm.Models;
using App.Areas.Crm.Repositories.Email;
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
    /// Email API
    /// </summary>
    [Authorize]
    [AreaName("Crm")]
    [Route("api/crm/email")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class EmailController : Controller
    {
        private readonly IEmailRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmailController> _logger;

        /// <summary>
        /// Controller for emails
        /// </summary>
        public EmailController(IEmailRepository repository, IMapper mapper, ILogger<EmailController> logger)
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
        /// Create a new email
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(EmailViewModel))]
        public async Task<EmailViewModel> Post([FromBody] EmailViewModel model)
        {
            _logger.LogInformation($"add.email contact: {model.ContactId}");
            AssertValidIds(model.ContactId);
            var data = _mapper.Map<Email>(model);
            var saved = await _repository.CreateAsync(data);
            _logger.LogInformation($"added.email contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<EmailViewModel>(saved);
        }


        /// <summary>
        /// Updates an email
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(EmailViewModel))]
        public async Task<EmailViewModel> Put([FromBody] EmailViewModel model)
        {
            _logger.LogInformation($"update.email contact: {model.ContactId} record: {model.Id}");
            AssertValidIds(model.ContactId, model.Id);
            var data = _mapper.Map<Email>(model);
            var saved = await _repository.UpdateAsync(data);
            _logger.LogInformation($"updated.email contact: {model.ContactId} record: {saved.Id}");
            return _mapper.Map<EmailViewModel>(saved);
        }

        /// <summary>
        /// Deletes an email
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        [HttpDelete("{contactId}/{id}")]
        public async Task Delete(Guid contactId, Guid id)
        {
            _logger.LogInformation($"delete.email contact: {id} record: {id}");
            AssertValidIds(contactId, id);
            var resp = await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.email contact: contact: {id} record: {id} resp: {resp}");
        }
    }
}