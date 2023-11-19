using Core.Application.DTOs;

namespace Core.Application.Services.Interfaces
{
    public interface ISettingService
    {
        Task<List<SettingDto>> GetSettingsAsync();
        Task<SettingDto> CreateSettingAsync(SettingDto setting);
        Task<SettingDto> GetSettingAsync(string name);
    }
}
