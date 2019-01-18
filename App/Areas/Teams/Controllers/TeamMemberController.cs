using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Crm.Services;
using App.Areas.Teams.Models;
using App.Areas.Teams.Repositories;
using App.Areas.Teams.ViewModels;
using AutoMapper;
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
    [Route("api/teammembers")]
    public class TeamMemberController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITeamMemberRepository _repository;
        private readonly IContactService _contactService;
        private readonly ILogger<TeamMemberController> _logger;
        private readonly IMapper _mapper;

        public TeamMemberController(
            IHttpContextAccessor httpContextAccessor,
            ITeamMemberRepository repository,IContactService contactService,
            ILogger<TeamMemberController> logger, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _contactService = contactService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches teamMembers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<TeamMemberViewModel>))]
        public async Task<IEnumerable<TeamMemberViewModel>> Search(TeamMemberSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"teamMembers.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.teamMembers {data.Count}");
            var contactIds = data.Select(it => it.ContactId);
            var contactsMap = await _contactService.GetNamesByIdAsync(contactIds.ToList());
            return data.Select(it =>
            {
                var contact = contactsMap[it.ContactId];
                var view = _mapper.Map<TeamMemberViewModel>(it);
                view.ContactName = contact.FullName;
                view.ContactAvatar = contact.Avatar;
                return view;
            });
        }

        /// <summary>
        /// Gets members of a specific team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns></returns>
        [HttpGet("{teamId}")]
        [Produces(typeof(IEnumerable<TeamMemberViewModel>))]
        public async Task<IEnumerable<TeamMemberViewModel>> Get(Guid teamId)
        {
            var request = new TeamMemberSearchRequest {TeamId = teamId };
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"teamMembers.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.teamMembers {data.Count}");
            var contactIds = data.Select(it => it.ContactId);
            var contactsMap = await _contactService.GetNamesByIdAsync(contactIds.ToList());
            return data.Select(it =>
            {
                var contact = contactsMap[it.ContactId];
                var view = _mapper.Map<TeamMemberViewModel>(it);
                view.ContactName = contact.FullName;
                view.ContactAvatar = contact.Avatar;
                return view;
            });
        }

        /// <summary>
        /// Create a teamMember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(TeamMemberViewModel))]
        public async Task<TeamMemberViewModel> Create([FromBody] TeamMemberViewModel model)
        {
            Tk.AssertValidIds(model.ContactId, model.TeamId);
            var (userId, _) = _httpContextAccessor.GetUser();
            model.CreatedBy = userId;
            _logger.LogInformation($"create.teamMember ${model.ContactId} ${model.TeamId}");
            var toSave = _mapper.Map<TeamMember>(model);
            var data = await _repository.CreateAsync(toSave);
            _logger.LogInformation($"created.teamMember ${data.Id}");
            return _mapper.Map<TeamMemberViewModel>(data);
            ;
        }

        /// <summary>
        /// Updates a teamMember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(TeamMemberViewModel))]
        public async Task<TeamMemberViewModel> Update([FromBody] TeamMemberViewModel model)
        {
            _logger.LogInformation($"update.teamMember ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var toSave = _mapper.Map<TeamMember>(model);
            var data = await _repository.UpdateAsync(toSave);
            _logger.LogInformation($"updated.teamMember ${data.Id}");
            return _mapper.Map<TeamMemberViewModel>(data);
            ;
        }

        /// <summary>
        /// Deletes a teamMember
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.teamMember ${id}");
            Tk.AssertValidIds(id);
            var data = await _repository.DeleteAsync(id);
            _logger.LogInformation($"delete.teamMember ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }
    }
}