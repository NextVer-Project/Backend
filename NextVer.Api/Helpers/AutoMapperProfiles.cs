using AutoMapper;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;

namespace NextVerBackend.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserForLoginDto, User>();
        }
    }
}