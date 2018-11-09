using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;

namespace App.Areas.Events.Services.Event
{
    public interface IEventService 
    {
        Task<EventViewModel> CreateAsync(EventViewModel entry);
        Task<EventViewModel> GetByIdAsync(Guid id);
        Task<int> DeleteAsync(Guid id);
        Task<EventViewModel> UpdateAsync(EventViewModel entry);
        Task<IEnumerable<EventViewModel>> SearchAsync(EventSearchRequest request);
    }
}
