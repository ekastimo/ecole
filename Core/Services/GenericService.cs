using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Repositories;

namespace Core.Services
{
    public class GenericService<TEntity, TViewModel, TSearchRequest> : IGenericService<TEntity, TViewModel, TSearchRequest>
        where TEntity : class where TViewModel : class where TSearchRequest : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TViewModel> CreateAsync(TViewModel entry)
        {
            var data = _mapper.Map<TEntity>(entry);
            data = await _repository.CreateAsync(data);
            return _mapper.Map<TViewModel>(data);
        }

        public async Task<IEnumerable<TViewModel>> CreateBatchAsync(IEnumerable<TViewModel> entries)
        {
            var data = entries.Select(it => _mapper.Map<TEntity>(it)).ToList();
            var created = await _repository.CreateBatchAsync(data);
            return _mapper.Map<IEnumerable<TViewModel>>(created);
        }

        public async Task<TViewModel> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return _mapper.Map<TViewModel>(result);
        }

        public async Task<TViewModel> GetDetailsByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return _mapper.Map<TViewModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<TViewModel> UpdateAsync(TViewModel entry)
        {
            var data = _mapper.Map<TEntity>(entry);
            var result = await _repository.UpdateAsync(data);
            return _mapper.Map<TViewModel>(result);
        }

        public Task<IEnumerable<TViewModel>> SearchAsync(TSearchRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TViewModel>> GetDetailsAsync(TSearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
