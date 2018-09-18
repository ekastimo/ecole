using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> CreateBatchAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                var batch = entities as IList<TEntity> ?? entities.ToList();
                await _context.Set<TEntity>().AddRangeAsync(batch);
                await _context.SaveChangesAsync();
                return batch;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        private Guid GetId(TEntity entity)
        {
            try
            {
                var type = entity.GetType();
                var prop = type.GetProperty("Id");
                var value = prop.GetValue(entity);
                var toRet = value as Guid?;
                return toRet ?? throw new InvalidDataException("Invalid record, for update");
            }
            catch (Exception)
            {
                throw new InvalidDataException("Invalid record, for update");
            }
        }

        private void SetLastUpdated(TEntity entity)
        {
            try
            {
                var type = entity.GetType();
                var prop = type.GetProperty("LastUpdated");
                prop.SetValue(entity, DateTime.Now);
            }
            catch (Exception)
            {
                throw new InvalidDataException("Cant change lastUpdated");
            }
        }

        private async Task AssertRecordExists(TEntity entity)
        {
            try
            {
                var id = GetId(entity);
                var rec = await GetByIdAsync(id);
                if (rec == null)
                    throw new InvalidDataException("Record not found");
            }
            catch (Exception)
            {
                throw new InvalidDataException("Record not found");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            SetLastUpdated(entity);
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> MatchesConditionAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await _context.Set<TEntity>().AnyAsync(condition);
        }
    }
}