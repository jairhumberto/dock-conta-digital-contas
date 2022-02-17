using AutoMapper;
using ContasService.Dtos;
using ContasService.Models;

namespace ContasService.Profiles
{
    public class PortadoresProfile : Profile
    {
        public PortadoresProfile()
        {
            CreateMap<Portador, PortadorReadDto>();
            CreateMap<PortadorCreateDto, Portador>();
        }
    }
}