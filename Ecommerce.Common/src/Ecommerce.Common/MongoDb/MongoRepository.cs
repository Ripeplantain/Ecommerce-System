using System.Linq.Expressions;
using MongoDB.Driver;


namespace Ecommerce.Common.MongoDB
{
    public class MongoRespository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _dbCollection;
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoRespository(IMongoClient mongoClient, string databaseName ,string collectionName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            return _dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
}