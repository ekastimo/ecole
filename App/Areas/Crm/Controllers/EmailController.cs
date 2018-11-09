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
    /// Email API
    /// </summary>
    [AreaName("Crm")]
    [Route("api/crm/email")]
    public class EmailController : BaseController
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

        /// <summary>
        /// Create a new email
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPost("{contactId}")]
        [Produces(typeof(EmailViewModel))]
        public async Task<EmailViewModel> Post(Guid contactId, [FromBody] EmailViewModel model)
        {
            _logger.LogInformation($"add.email contact: {contactId}");
            AssertValidIds(contactId);
            var data = _mapper.Map<Email>(model);
            var saved = await _repository.CreateAsync(contactId, data);
            _logger.LogInformation($"added.email contact: {contactId} record: {saved.Id}");
            return _mapper.Map<EmailViewModel>(saved);
        }


        /// <summary>
        /// Updates an email
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        [HttpPut("{contactId}")]
        [Produces(typeof(EmailViewModel))]
        public async Task<EmailViewModel> Put(Guid contactId, [FromBody] EmailViewModel model)
        {
            _logger.LogInformation($"update.email contact: {contactId} record: {model.Id}");
            AssertValidIds(contactId, model.Id);
            var data = _mapper.Map<Email>(model);
            var saved = await _repository.UpdateAsync(contactId, data);
            _logger.LogInformation($"updated.email contact: {contactId} record: {saved.Id}");
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
            var resp = await _repository.DeleteAsync(contactId, id);
            _logger.LogInformation($"deleted.email contact: {id} record: {id} resp: {resp}");
        }
    }
}