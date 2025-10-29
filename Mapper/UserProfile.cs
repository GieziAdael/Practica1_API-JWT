using AutoMapper;
using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;

namespace Practica_API_JWT.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
        }
    }
}
