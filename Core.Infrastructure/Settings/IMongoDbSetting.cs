namespace Core.Infrastructure.Settings
{
    public interface IMongoDbSetting
    {
        string Database { get; set; }
        string ConnectionString { get; set; }
    }
}
