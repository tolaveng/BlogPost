using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Core.Domain.Documents;
using Core.Domain.IRepositories;

namespace Core.Application.Services
{
    public class SettingService : ISettingService
    {
        private readonly IMongoRepository<Setting> repository;
        private readonly IMapper mapper;

        public SettingService(IMongoRepository<Setting> mongoRepository, IMapper mapper)
        {
            this.repository = mongoRepository;
            this.mapper = mapper;
        }

        public async Task<SettingDto> GetSettingAsync(string name)
        {
            var setting = await repository.FindOneAsync(x => x.Name == name).ConfigureAwait(false);
            return mapper.Map<SettingDto>(setting);
        }

        public async Task<List<SettingDto>> GetSettingsAsync()
        {
            var settings = repository.AsQueryable().ToList();
            return mapper.Map<List<SettingDto>>(settings);
        }

        public async Task<SettingDto> CreateSettingAsync(SettingDto setting)
        {
            try
            {
                var doc = mapper.Map<Setting>(setting);
                await repository.InsertOneAsync(doc);
                return setting;
            } catch (Exception ex)
            {
                return null;
            }
        }
    }
}
