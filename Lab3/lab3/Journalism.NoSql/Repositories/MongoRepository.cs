using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Journalism.NoSql.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(T).Name.ToLower() + "s";
            }

            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Empty;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException("Entity must have Id property");

            var id = idProperty.GetValue(entity);
            if (id == null)
                throw new InvalidOperationException("Id must not be null");

            var filter = Builders<T>.Filter.Eq("Id", id.ToString());
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
