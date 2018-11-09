using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using MongoDB.Driver;

namespace Core.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;


        public GenericRepository(IMongoCollection<TEntity> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, int skip = 0,
            int limit = 100)
        {
            return await _collection.Find(filter).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                return await _collection.Find(filter).FirstAsync();
            }
            catch (Exception )
            {
                throw new NotFoundException($"Invalid record {id}");
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> CreateBatchAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                var batch = entities as IList<TEntity> ?? entities.ToList();
                await _collection.InsertManyAsync(batch);
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
            var id = GetId(entity);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            SetLastUpdated(entity);
            await _collection.ReplaceOneAsync(filter, entity);
            return entity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return (int) result.DeletedCount;
        }

        public async Task<bool> MatchesConditionAsync(FilterDefinition<TEntity> filter)
        {
            var result = await _collection.CountAsync(filter);
            return result > 0;
        }
    }
}