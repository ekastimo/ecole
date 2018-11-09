using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IGenericService<TEntity, TViewModel, in TSearchRequest> where TEntity : class where TViewModel : class where TSearchRequest : class
    {
        Task<TViewModel> CreateAsync(TViewModel entry);
        Task<IEnumerable<TViewModel>> CreateBatchAsync(IEnumerable<TViewModel> entries);
        Task<TViewModel> GetByIdAsync(Guid id);
        Task<TViewModel> GetDetailsByIdAsync(Guid id);
        Task<IEnumerable<TViewModel>> GetDetailsAsync(TSearchRequest request);
        Task DeleteAsync(Guid id);
        Task<TViewModel> UpdateAsync(TViewModel entry);
        Task<IEnumerable<TViewModel>> SearchAsync(TSearchRequest request);
    }
}
