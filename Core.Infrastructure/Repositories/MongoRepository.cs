using Core.Domain.Attributes;
using Core.Domain.Documents;
using Core.Domain.IRepositories;
using Core.Infrastructure.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Infrastructure.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoHelper mongoHelper)
        {
            var database = mongoHelper.Database;
            var collectionAtt = typeof(TDocument).GetCustomAttributes(typeof(MongoDbCollectionAttribute), false).FirstOrDefault() as MongoDbCollectionAttribute;
            if (collectionAtt == null)
            {
                throw new ArgumentException("MongoDb collection attribute not found");
            }

            _collection = database.GetCollection<TDocument>(collectionAtt.CollectionName);

            var indexes = typeof(TDocument).GetCustomAttributes(typeof(MongoDbIndexesAttribute), false);
            if (indexes != null && indexes.Any())
            {
                foreach (var index in indexes)
                {
                    if (index is MongoDbIndexesAttribute)
                    {
                        var att = index as MongoDbIndexesAttribute;
                        var keys = Builders<TDocument>.IndexKeys.Ascending(att.Index);
                        var options = new CreateIndexOptions { Unique = true };
                        _collection.Indexes?.CreateOne(new CreateIndexModel<TDocument>(keys, options));
                    }
                }
            }
        }

        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public async Task<TDocument> FindByIdAsync(ObjectId id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task<bool> PingAsync()
        {
            try {
                var ping = _collection.Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
                return ping;
            } catch (Exception) {
                return false;
            }
        }

        public async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }
    }
}
