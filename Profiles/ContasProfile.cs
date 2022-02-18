using AutoMapper;
using ContasService.Dtos;
using ContasService.Models;

namespace ContasService.Profiles
{
    public class ContasProfile : Profile
    {
        public ContasProfile()
        {
            CreateMap<Conta, ContaReadDto>();
            CreateMap<ContaCreateDto, Conta>().AfterMap((o,n) => {
                n.Ativa = true;
            });
            CreateMap<Conta, ContaUpdateDto>();
            CreateMap<ContaUpdateDto, Conta>();
        }
    }
}