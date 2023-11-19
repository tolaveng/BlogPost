using MongoDB.Driver;

namespace Core.Infrastructure.MongoDb
{
    public interface IMongoHelper
    {
        MongoClient MongoClient { get; }
        MongoDatabaseBase Database { get; }
    }
}
