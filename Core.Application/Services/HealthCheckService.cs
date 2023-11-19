using Core.Application.Services.Interfaces;
using Core.Domain.Documents;
using Core.Domain.IRepositories;

namespace Core.Application.Services
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IMongoRepository<Setting> mongoRespository;

        public HealthCheckService(IMongoRepository<Setting> mongoRespository)
        {
            this.mongoRespository = mongoRespository;
        }

        public async Task<bool> CanConnectToAzureStorage()
        {
            return false;
        }

        public async Task<bool> CanConnectToMongoDb()
        {
            try
            {
                return await mongoRespository.PingAsync();
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
