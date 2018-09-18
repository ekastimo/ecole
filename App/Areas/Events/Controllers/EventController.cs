using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Events.Services;
using App.Areas.Events.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Events.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// All end-points dealing with events 
    /// </summary>
    [Authorize]
    [AreaName("Events")]
    [Route("api/evt/event")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(500)]
    public class EventController : Controller
    {
        private readonly IEventService _service;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService service, ILogger<EventController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Searches events
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<EventViewModel>))]
        public async Task<IEnumerable<EventViewModel>> Search(EventSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"search.event ${json}");
            var data = (await _service.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.event {data.Count}");
            return data;
        }

        /// <summary>
        /// Deletes a event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<object> Get(Guid id)
        {
            _logger.LogInformation($"get.event ${id}");
            Tk.AssertValidIds(id);
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                throw new NotFoundException($"invalid record id: {id}");
            _logger.LogInformation($"got.event ${data.Id}");
            return data;
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(EventViewModel))]
        public async Task<EventViewModel> Create([FromBody] EventViewModel model)
        {
            _logger.LogInformation("add.event");
            var data = await _service.CreateAsync(model);
            _logger.LogInformation($"added.event {data.Id}");
            return data;
        }


        /// <summary>
        /// Updates a event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(EventViewModel))]
        public async Task<EventViewModel> Update([FromBody] EventViewModel model)
        {
            _logger.LogInformation($"update.event ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var data = await _service.UpdateAsync(model);
            _logger.LogInformation($"updated.event ${data.Id}");
            return data;
        }

        /// <summary>
        /// Deletes a event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.event ${id}");
            Tk.AssertValidIds(id);
            var data = await _service.DeleteAsync(id);
            _logger.LogInformation($"delete.event ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }
    }
}