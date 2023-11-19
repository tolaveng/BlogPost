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
            CreateMap<User, UserDto>()
                .ForMember(x => x.Password, x => x.MapFrom(z => ""));
        }
    }
}
