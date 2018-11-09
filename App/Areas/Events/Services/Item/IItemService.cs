using System;
using System.Threading.Tasks;
using App.Areas.Events.ViewModels;

namespace App.Areas.Events.Services.EventItem
{
    public interface IItemService
    {
        Task<ItemViewModel> CreateAsync(ItemViewModel entry);
        Task<int> DeleteAsync(Guid parentId, Guid id);
        Task<ItemViewModel> UpdateAsync(ItemViewModel entry);
    }
}