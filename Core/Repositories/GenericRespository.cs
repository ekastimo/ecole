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

        public async Task<TEntity> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                return await _collection.Find(filter).FirstAsync();
            }
            catch (Exception)
            {
                throw new NotFoundException($"Invalid record {id}");
            }
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                return await _collection.Find(filter).FirstAsync();
            }
            catch (Exception)
            {
                throw new NotFoundException($"Invalid record {id}");
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            SetCreatedAt(entity);
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

        private object GetId(TEntity entity)
        {
            try
            {
                var type = entity.GetType();
                var prop = type.GetProperty("Id");
                var value = prop?.GetValue(entity);
                return value ?? throw new ClientFriendlyException("Invalid record, for update");
            }
            catch (Exception)
            {
                throw new ClientFriendlyException("Invalid record, for update");
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
                throw new ClientFriendlyException("Cant change lastUpdated");
            }
        }

        private void SetCreatedAt(TEntity entity)
        {
            try
            {
                var type = entity.GetType();
                var prop = type.GetProperty("CreatedAt");
                prop?.SetValue(entity, DateTime.Now);
            }
            catch (Exception)
            {
                throw new ClientFriendlyException("Cant change CreatedAt");
            }
        }

        private async Task AssertRecordExists(TEntity entity)
        {
            var exception = new ClientFriendlyException("Record not found");
            try
            {
                var id = GetId(entity);
                object rec;
                switch (id)
                {
                    case Guid g:
                        rec = await GetByIdAsync(g);
                        break;
                    case string s:
                        rec = await GetByIdAsync(s);
                        break;
                    default:
                        rec = null;
                        break;
                }

                if (rec == null)
                    throw exception;
            }
            catch (Exception)
            {
                throw exception;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var id = GetId(entity);
            var filter = Builders<TEntity>.Filter.Empty;
            switch (id)
            {
                case string idStr:
                    filter = Builders<TEntity>.Filter.Eq("_id", idStr);
                    break;
                case Guid idGuid:
                    filter = Builders<TEntity>.Filter.Eq("_id", idGuid);
                    break;
            }

            SetLastUpdated(entity);
            var result = await _collection.ReplaceOneAsync(filter, entity);
            if (result.ModifiedCount <= 0)
            {
                throw new ClientFriendlyException("Record not updated");
            }

            return entity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return (int) result.DeletedCount;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return (int) result.DeletedCount;
        }

        public async Task<bool> MatchesConditionAsync(FilterDefinition<TEntity> filter)
        {
            var result = await _collection.CountDocumentsAsync(filter);
            return result > 0;
        }
    }
}