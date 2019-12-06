using App.Areas.Crm.Services;
using App.Areas.Groups.Models;
using App.Areas.Groups.Repositories;
using App.Areas.Groups.ViewModels;
using AutoMapper;
using Core.Controllers;
using Core.Extensions;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Groups.Controllers
{
    /// <summary>
    /// Members Controller
    /// </summary>
    [Route("api/groups/members")]
    public class MemberController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemberRepository _repository;
        private readonly IGroupRepository _groupRepository;
        private readonly IContactService _contactService;
        private readonly ILogger<MemberController> _logger;
        private readonly IMapper _mapper;

        public MemberController(
            IHttpContextAccessor httpContextAccessor,
            IMemberRepository repository, IGroupRepository groupRepository, IContactService contactService,
            ILogger<MemberController> logger, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _groupRepository = groupRepository;
            _contactService = contactService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches members
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IEnumerable<MemberViewModel>))]
        public async Task<IEnumerable<MemberViewModel>> Search(MemberSearchRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"members.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.members {data.Count}");
            var contactIds = data.Select(it => it.ContactId);
            var contactsMap = await _contactService.GetNamesByIdAsync(contactIds.ToList());
            return data.Select(it =>
            {
                var contact = contactsMap[it.ContactId];
                var view = _mapper.Map<MemberViewModel>(it);
                view.ContactName = contact.FullName;
                view.ContactAvatar = contact.Avatar;
                return view;
            });
        }

        /// <summary>
        /// Gets members of a specific group
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <returns></returns>
        [HttpGet("group/{groupId}")]
        [Produces(typeof(IEnumerable<MemberViewModel>))]
        public async Task<IEnumerable<MemberViewModel>> Get(Guid groupId)
        {
            var request = new MemberSearchRequest { GroupId = groupId };
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"members.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.members {data.Count}");
            var contactIds = data.Select(it => it.ContactId);
            var contactsMap = await _contactService.GetNamesByIdAsync(contactIds.ToList());
            return data.Select(it =>
            {
                var contact = contactsMap.SafeGet(it.ContactId);
                var view = _mapper.Map<MemberViewModel>(it);
                view.ContactName = contact?.FullName;
                view.ContactAvatar = contact?.Avatar;
                return view;
            }).ToList();
        }

        /// <summary>
        /// Gets groups of a specific contact
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        [HttpGet("contact/{contactId}")]
        [Produces(typeof(IEnumerable<object>))]
        public async Task<IEnumerable<object>> GetGroups(Guid contactId)
        {
            var request = new MemberSearchRequest { ContactId = contactId };
            var json = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"groups.by.query ${json}");
            var data = (await _repository.SearchAsync(request)).ToList();
            _logger.LogInformation($"found.groups {data.Count}");
            var groupIds = data.Select(it => it.GroupId);
            var groups = await _groupRepository.SearchByIdsAsync(groupIds.ToList());
            var dict = groups.ToImmutableDictionary(x => x.Id, x => x);
            return data.Select(it => new
            {
                it.Id,
                it.GroupId,
                it.ContactId,
                it.Role,
                dict.SafeGet(it.GroupId).Name,
                dict.SafeGet(it.GroupId).Description
            }).ToList();
        }


        /// <summary>
        /// Attach contact to specific group
        /// </summary>
        /// <param name="model">Simple Member Model</param>
        /// <returns></returns>
        [HttpPost("contact")]
        [Produces(typeof(IEnumerable<object>))]
        public async Task<MemberViewModel> CreateMember([FromBody] MemberViewModel model)
        {
            Tk.AssertValidIds(model.GroupId,model.ContactId);
            var userId = _httpContextAccessor.GetContactId();
            model.CreatedBy = userId;
            _logger.LogInformation($"create.groupMember Group: {model.GroupId} Contact: {model.ContactId}");
            var toSave = _mapper.Map<Member>(model);
            var data = await _repository.CreateAsync(toSave);
            _logger.LogInformation($"created.groupMember ${data.Id}");
            return _mapper.Map<MemberViewModel>(data);
        }


        /// <summary>
        /// Update contact on a specific group
        /// </summary>
        /// <param name="model">Simple Member Model</param>
        /// <returns></returns>
        [HttpPut("contact")]
        [Produces(typeof(IEnumerable<object>))]
        public async Task<MemberViewModel> UpdateMember([FromBody] MemberViewModel model)
        {
            Tk.AssertValidIds(model.GroupId, model.ContactId);
            _logger.LogInformation($"update.groupMember Group: {model.GroupId} Contact: {model.ContactId}");
            var toSave = _mapper.Map<Member>(model);
            var data = await _repository.UpdateAsync(toSave);
            _logger.LogInformation($"updated.groupMember ${data.Id}");
            return _mapper.Map<MemberViewModel>(data);
        }

        /// <summary>
        /// Create a groupMember
        /// </summary>
        /// <param name="model">Extend model to add multiple members</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(CreateMembersViewModel))]
        public async Task<CreateMembersViewModel> Create([FromBody] CreateMembersViewModel model)
        {
            Tk.AssertValidIds(model.GroupId);
            Tk.AssertValidIds(model.ContactIds.ToArray());
            var userId = _httpContextAccessor.GetContactId();
            model.CreatedBy = userId;
            _logger.LogInformation($"create.groupMember ${model.GroupId}");
            var toSave = model.ContactIds.Select(it => new Member
            {
                GroupId = model.GroupId,
                ContactId = it,
                Role = model.Role,
                Status = GroupStatus.Active
            }).ToList();
            var data = await _repository.CreateBatchAsync(toSave);
            _logger.LogInformation($"created.groupMember ${data.Count()}");
            return model;
        }

        /// <summary>
        /// Updates a groupMember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(MemberViewModel))]
        public async Task<MemberViewModel> Update([FromBody] MemberViewModel model)
        {
            _logger.LogInformation($"update.groupMember ${model.Id}");
            Tk.AssertValidIds(model.Id);
            var toSave = _mapper.Map<Member>(model);
            var data = await _repository.UpdateAsync(toSave);
            _logger.LogInformation($"updated.groupMember ${data.Id}");
            return _mapper.Map<MemberViewModel>(data);
            ;
        }

        /// <summary>
        /// Deletes a groupMember
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            _logger.LogInformation($"delete.groupMember ${id}");
            Tk.AssertValidIds(id);
            var data = await _repository.DeleteAsync(id);
            _logger.LogInformation($"delete.groupMember ${id} result: {data}");
            return new
            {
                Message = "Operation successful"
            };
        }
    }
}