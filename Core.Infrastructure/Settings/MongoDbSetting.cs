namespace Core.Infrastructure.Settings
{
    public class MongoDbSetting : IMongoDbSetting
    {
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
