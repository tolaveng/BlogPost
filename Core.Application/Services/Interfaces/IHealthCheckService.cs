namespace Core.Application.Services.Interfaces
{
    public interface IHealthCheckService
    {
        public Task<bool> CanConnectToMongoDb();
        public Task<bool> CanConnectToAzureStorage();
    }
}
