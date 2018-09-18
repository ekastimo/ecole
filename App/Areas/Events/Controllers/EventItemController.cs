using System;
using System.Threading.Tasks;
using App.Areas.Events.Models;
using App.Areas.Events.Repositories.AgendaItem;
using App.Areas.Events.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Events.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// AgendaItem API
    /// </summary>
    [Authorize]
    [AreaName("Events")]
    [Route("api/evt/items")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class AgendaItemController : Controller
    {
        private readonly IEventItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AgendaItemController> _logger;

        /// <summary>
        /// Controller for event-items
        /// </summary>
        public AgendaItemController(IEventItemRepository repository, IMapper mapper, ILogger<AgendaItemController> logger)
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
        /// Create a new event-item
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [Produces(typeof(EventItemViewModel))]
        public async Task<EventItemViewModel> Post([FromBody] EventItemViewModel model)
        {
            _logger.LogInformation($"add.event-item contact: {model.EventId}");
            AssertValidIds(model.EventId);
            var data = _mapper.Map<EventItem>(model);
            var saved = await _repository.CreateAsync(data);
            _logger.LogInformation($"added.event-item contact: {model.EventId} record: {saved.Id}");
            return _mapper.Map<EventItemViewModel>(saved);
        }


        /// <summary>
        /// Updates an event-item
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        [Produces(typeof(EventItemViewModel))]
        public async Task<EventItemViewModel> Put([FromBody] EventItemViewModel model)
        {
            _logger.LogInformation($"update.event-item contact: {model.EventId} record: {model.Id}");
            AssertValidIds(model.EventId, model.Id);
            var data = _mapper.Map<EventItem>(model);
            var saved = await _repository.UpdateAsync(data);
            _logger.LogInformation($"updated.event-item contact: {model.EventId} record: {saved.Id}");
            return _mapper.Map<EventItemViewModel>(saved);
        }

        /// <summary>
        /// Deletes an event-item
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="id"></param>
        [HttpDelete("{eventId}/{id}")]
        public async Task Delete(Guid eventId, Guid id)
        {
            _logger.LogInformation($"delete.event-item contact: {id} record: {id}");
            AssertValidIds(eventId, id);
            var resp = await _repository.DeleteAsync(id);
            _logger.LogInformation($"deleted.event-item contact: contact: {id} record: {id} resp: {resp}");
        }
    }
}