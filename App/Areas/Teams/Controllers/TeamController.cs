using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Teams.Services;
using App.Areas.Teams.ViewModels;
using Core.Controllers;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Areas.Teams.Controllers
{
    /// <summary>
    /// Documents API
    /// </summary>
    [AreaName("Teams")]
    [Route("api/teams/team")]
    public class TeamController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITeamService _service;
        private readonly ILogger<TeamController> _logger;

        public TeamController(IHttpContextAccessor httpContextAccessor, ITeamService service,
            ILogger<TeamController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Searches teams
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<TeamViewModel>))]
        public async Task<IEnumerable<TeamViewModel>> Search(TeamSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"teams.by.query ${json}");
            var data = (await _service.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.teams {data.Count}");
            return data;
        }

        /// <summary>
        /// Gets a specific team
        /// </summary>
        /// <param name="id">Team Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(typeof(TeamViewModel))]
        public async Task<TeamViewModel> Get(Guid id)
        {
            _logger.LogInformation($"team.by.id ${id}");
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                throw new NotFoundException($"Invalid record id:{id}");
            _logger.LogInformation($"found.team {data.Id}");
            return data;
        }

        /// <summary>
        /// Create a team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(TeamViewModel))]
        public async Task<TeamViewModel> Create([FromBody] TeamViewModel model)
        {
            model.CreatedBy = _httpContextAccessor.GetContactId();
            _logger.LogInformation($"create.team ${model.Name}");
            var data = await _service.CreateAsync(model);
            _logger.LogInformation($"created.team ${data.Id}");
            return data;
        }

        /// <summary>
        /// Updates a team
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(TeamViewModel))]
        public async Task<TeamViewModel> Update([FromBody] TeamViewModel model)
        {
            _logger.LogInformation($"update.team ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var data = await _service.UpdateAsync(model);
            _logger.LogInformation($"updated.team ${data.Id}");
            return data;
        }

        /// <summary>
        /// Deletes a team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.team ${id}");
            Tk.AssertValidIds(id);
            var data = await _service.DeleteAsync(id);
            _logger.LogInformation($"delete.team ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }
    }
}