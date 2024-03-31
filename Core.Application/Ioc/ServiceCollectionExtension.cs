using Core.Application.Mapper;
using Core.Application.Services;
using Core.Application.Services.Interfaces;
using Core.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Ioc
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHealthCheckService, HealthCheckService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.Configure<AzureStorageSetting>(configuration.GetSection("AzureStorage"));
            services.Configure<CloudinarySetting>(configuration.GetSection("Cloudinary"));

            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            //services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IFileUploadService, CloudinaryFileUploadService>();
        }
    }
}
