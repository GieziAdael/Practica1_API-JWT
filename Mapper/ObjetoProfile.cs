using AutoMapper;
using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;

namespace Practica_API_JWT.Mapper
{
    public class ObjetoProfile:Profile
    {
        public ObjetoProfile()
        {
            CreateMap<Objeto, ObjetoDto>().ReverseMap();
            CreateMap<Objeto, ObjetoCreateDto>().ReverseMap();
        }
    }
}
