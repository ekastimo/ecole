using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.Repositories.Event;
using App.Areas.Events.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.Areas.Events.Services.Event
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(IEventRepository repository, IMapper mapper, ILogger<EventService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<EventViewModel> CreateAsync(EventViewModel entry)
        {
            var data = _mapper.Map<Models.Event>(entry);
            var saved = await _repository.CreateAsync(data);
            var created = _mapper.Map<EventViewModel>(saved);
            return created;
        }

        public async Task<EventViewModel> GetByIdAsync(Guid id)
        {
            var data = await _repository.GetByIdAsync(id);
            return _mapper.Map<EventViewModel>(data);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<EventViewModel> UpdateAsync(EventViewModel entry)
        {
            var data = _mapper.Map<Models.Event>(entry);
            var result = await _repository.UpdateAsync(data);
            return _mapper.Map<EventViewModel>(result);
        }

        public async Task<IEnumerable<EventViewModel>> SearchAsync(EventSearchRequest request)
        {
            var result = await _repository.SearchAsync(request);
            return _mapper.Map<IEnumerable<EventViewModel>>(result);
        }
    }
}