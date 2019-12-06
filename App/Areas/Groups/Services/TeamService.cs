using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.Repositories;
using App.Areas.Crm.Services;
using App.Areas.Groups.Models;
using App.Areas.Groups.Repositories;
using App.Areas.Groups.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace App.Areas.Groups.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupViewModel>> SearchAsync(GroupSearchRequest request);
        Task<GroupViewModel> CreateAsync(GroupViewModel model);
        Task<GroupViewModel> UpdateAsync(GroupViewModel model);
        Task<GroupViewModel> DeleteAsync(Guid teamId);
        Task<bool> GroupExistsByNameAsync(string teamName);
        Task<GroupViewModel> GetByIdAsync(Guid id);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IContactRepository _contactRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public GroupService(
            IGroupRepository repository,
            IContactRepository contactRepository,
            IMapper mapper, ILogger<ContactService> logger)
        {
            _repository = repository;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<GroupViewModel> CreateAsync(GroupViewModel model)
        {
            var contactExists = await GroupExistsByNameAsync(model.Name);
            if (contactExists)
            {
                throw new ClientFriendlyException($"Group: {model.Name} already exists");
            }

            var data = _mapper.Map<Group>(model);
            var result = await _repository.CreateAsync(data);
            _logger.LogInformation($"Created new team {model.Id} {model.Name}");
            return _mapper.Map<GroupViewModel>(result);
        }

        public async Task<GroupViewModel> UpdateAsync(GroupViewModel model)
        {
            var team = await _repository.GetByIdAsync(model.Id);
            if (team == null)
            {
                throw new ClientFriendlyException($"Invalid Group: {model.Id}");
            }

            var data = _mapper.Map<Group>(model);
            var result = await _repository.UpdateAsync(data);
            _logger.LogInformation($"Updated team {model.Id} {model.Name}");
            return _mapper.Map<GroupViewModel>(result);
        }


        public async Task<GroupViewModel> DeleteAsync(Guid teamId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
            {
                throw new ClientFriendlyException($"Invalid Group: {teamId}");
            }

            var result = await _repository.DeleteAsync(teamId);
            _logger.LogInformation($"deteted team {teamId}");
            return _mapper.Map<GroupViewModel>(result);
        }

        public async Task<bool> GroupExistsByNameAsync(string teamName)
        {
            return await _repository.ExistsByNameAsync(teamName);
        }

        public async Task<GroupViewModel> GetByIdAsync(Guid id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<GroupViewModel>(data);
        }

        public async Task<IEnumerable<GroupViewModel>> SearchAsync(GroupSearchRequest request)
        {
            var data = await _repository.SearchAsync(request);
            return _mapper.Map<IEnumerable<GroupViewModel>>(data);
        }
    }
}