using Core.Domain.IRepositories;
using Core.Infrastructure.MongoDb;
using Core.Infrastructure.Repositories;
using Core.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Core.Infrastructure.Ioc
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSetting>(configuration.GetSection("MongoDb"));
            services.AddSingleton<IMongoDbSetting>(sp => sp.GetRequiredService<IOptions<MongoDbSetting>>().Value);

            services.AddSingleton<IMongoHelper, MongoHelper>();
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        }
    }
}
