using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.Services;
using App.Areas.Teams.Models;
using App.Areas.Teams.Repositories;
using App.Areas.Teams.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace App.Areas.Teams.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamViewModel>> SearchAsync(TeamSearchRequest request);
        Task<TeamViewModel> CreateAsync(TeamViewModel model);
        Task<TeamViewModel> UpdateAsync(TeamViewModel model);
        Task<TeamViewModel> DeleteAsync(Guid teamId);
        Task<bool> TeamExistsByNameAsync(string teamName);
        Task<TeamViewModel> GetByIdAsync(Guid id);
    }

    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IContactRepository _contactRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public TeamService(
            ITeamRepository repository,
            IContactRepository contactRepository,
            IMapper mapper, ILogger<ContactService> logger)
        {
            _repository = repository;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<TeamViewModel> CreateAsync(TeamViewModel model)
        {
            var contactExists = await TeamExistsByNameAsync(model.Name);
            if (contactExists)
            {
                throw new ClientFriendlyException($"Team: {model.Name} already exists");
            }

            var data = _mapper.Map<Team>(model);
            var result = await _repository.CreateAsync(data);
            _logger.LogInformation($"Created new team {model.Id} {model.Name}");
            return _mapper.Map<TeamViewModel>(result);
        }

        public async Task<TeamViewModel> UpdateAsync(TeamViewModel model)
        {
            var team = await _repository.GetByIdAsync(model.Id);
            if (team == null)
            {
                throw new ClientFriendlyException($"Invalid Team: {model.Id}");
            }

            var data = _mapper.Map<Team>(model);
            var result = await _repository.UpdateAsync(data);
            _logger.LogInformation($"Updated team {model.Id} {model.Name}");
            return _mapper.Map<TeamViewModel>(result);
        }


        public async Task<TeamViewModel> DeleteAsync(Guid teamId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
            {
                throw new ClientFriendlyException($"Invalid Team: {teamId}");
            }

            var result = await _repository.DeleteAsync(teamId);
            _logger.LogInformation($"deteted team {teamId}");
            return _mapper.Map<TeamViewModel>(result);
        }

        public async Task<bool> TeamExistsByNameAsync(string teamName)
        {
            return await _repository.TeamExistsByNameAsync(teamName);
        }

        public async Task<TeamViewModel> GetByIdAsync(Guid id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<TeamViewModel>(data);
        }

        public async Task<IEnumerable<TeamViewModel>> SearchAsync(TeamSearchRequest request)
        {
            var data = await _repository.SearchAsync(request);
            return _mapper.Map<IEnumerable<TeamViewModel>>(data);
        }
    }
}