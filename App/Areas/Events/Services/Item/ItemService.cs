using System;
using System.Threading.Tasks;
using App.Areas.Events.Repositories.Item;
using App.Areas.Events.Services.EventItem;
using App.Areas.Events.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.Areas.Events.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;

        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IItemRepository repository,IMapper mapper, ILogger<ItemService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ItemViewModel> CreateAsync(ItemViewModel entry)
        {
            var data = _mapper.Map<Models.Item>(entry);
            var saved = await _repository.CreateAsync(entry.EventId,data);
            var created = _mapper.Map<ItemViewModel>(saved);
            return created;
        }

        public async Task<int> DeleteAsync(Guid eventId, Guid id)
        {
            return await _repository.DeleteAsync(eventId,id);
        }

        public async Task<ItemViewModel> UpdateAsync(ItemViewModel entry)
        {
            var data = _mapper.Map<Models.Item>(entry);
            var result = await _repository.UpdateAsync(entry.EventId,data);
            return _mapper.Map<ItemViewModel>(result);
        }

      
    }
}
