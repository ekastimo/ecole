using System;
using System.Threading.Tasks;
using App.Areas.Events.Services.EventItem;
using App.Areas.Events.ViewModels;
using AutoMapper;
using Core.Controllers;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Events.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// AgendaItem API
    /// </summary>
    [AreaName("Events")]
    [Route("api/evt/items")]
    public class AgendaItemController : BaseController
    {
        private readonly IItemService _repository;
        private readonly ILogger<AgendaItemController> _logger;

        /// <summary>
        /// Controller for event-items
        /// </summary>
        public AgendaItemController(IItemService repository, IMapper mapper, ILogger<AgendaItemController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        /// Create a new event-item
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(ItemViewModel))]
        public async Task<ItemViewModel> Post([FromBody] ItemViewModel model)
        {
            _logger.LogInformation($"add.event-item contact: {model.EventId}");
            AssertValidIds(model.EventId);
            var saved = await _repository.CreateAsync(model);
            _logger.LogInformation($"added.event-item contact: {model.EventId} record: {saved.Id}");
            return saved;
        }


        /// <summary>
        /// Updates an event-item
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(ItemViewModel))]
        public async Task<ItemViewModel> Put([FromBody] ItemViewModel model)
        {
            _logger.LogInformation($"update.event-item contact: {model.EventId} record: {model.Id}");
            AssertValidIds(model.EventId, model.Id);
            var saved = await _repository.UpdateAsync(model);
            _logger.LogInformation($"updated.event-item contact: {model.EventId} record: {saved.Id}");
            return saved;
        }

        /// <summary>
        /// Deletes an event-item
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="id"></param>
        [HttpDelete("{eventId}/{id}")]
        public async Task<object> Delete(Guid eventId, Guid id)
        {
            _logger.LogInformation($"delete.event-item contact: {id} record: {id}");
            AssertValidIds(eventId, id);
            var resp = await _repository.DeleteAsync(eventId, id);
            _logger.LogInformation($"deleted.event-item contact: contact: {id} record: {id} resp: {resp}");
            return new
            {
                Message = "Operation Successfull",
                Data = resp
            };
        }
    }
}