using Core.Application.Services;
using Core.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Ioc
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHealthCheckService, HealthCheckService>();
            //services.AddAutoMapper(typeof(AutoMapperProfile));
            //services.AddScoped<ISettingService, SettingService>();
            //services.AddScoped<IUserService, UserService>();
        }
    }
}
