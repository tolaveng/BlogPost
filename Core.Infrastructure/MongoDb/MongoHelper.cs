using Core.Infrastructure.Settings;
using MongoDB.Driver;

namespace Core.Infrastructure.MongoDb
{
    public class MongoHelper : IMongoHelper
    {
        private MongoClient client;
        private readonly IMongoDbSetting settings;

        public MongoHelper(IMongoDbSetting settings)
        {
            this.settings = settings;
        }

        public MongoClient MongoClient
        {
            get
            {
                if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                    throw new ArgumentNullException("Mongo connection string is not defined");

                if (client != null) return client;
                client = new MongoClient(settings.ConnectionString);
                return client;
            }
        }

        public MongoDatabaseBase Database
        {
            get
            {
                if (settings == null || settings.Database == null)
                    throw new ArgumentNullException("Mongo database is not defined");

                return (MongoDatabaseBase)MongoClient.GetDatabase(settings.Database);
            }
        }
    }
}
