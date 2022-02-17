using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContasService.Data;
using ContasService.Dtos;
using ContasService.Models;

namespace ContasService.Controllers
{
    [Route("api/cs/[controller]")]
    [ApiController]
    public class PortadoresController : ControllerBase
    {
        private readonly IPortadoresRepository _repository;
        private readonly IMapper _mapper;

        public PortadoresController(IPortadoresRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<PortadorReadDto> CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portadorModel = _repository.GetPortadorByCpf(portadorCreateDto.Cpf);

            if (portadorModel != null)
            {
                _repository.DeletePortador(portadorModel);
                _repository.SaveChanges();
            }

            portadorModel = _mapper.Map<Portador>(portadorCreateDto);

            _repository.CreatePortador(portadorModel);
            _repository.SaveChanges();

            return Ok(_mapper.Map<PortadorReadDto>(portadorModel));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PortadorReadDto>> GetPortadores()
        {
            var portadores = _repository.GetPortadores();
            return Ok(_mapper.Map<IEnumerable<PortadorReadDto>>(portadores));
        }
    }
}