using AutoMapper;
using Core.Application.DTOs;
using Core.Domain.Documents;

namespace Core.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SettingDto, Setting>().ReverseMap();
            CreateMap<UserDto, User>();
        }
    }
}
