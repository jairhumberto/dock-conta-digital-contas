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
        private readonly IPortadoresRepository _portadorRepository;
        private readonly IContasRepository _contasRepository;
        private readonly IMapper _mapper;

        public PortadoresController(IPortadoresRepository portadorRepository, IContasRepository contasRepository,
                IMapper mapper)
        {
            _portadorRepository = portadorRepository;
            _contasRepository = contasRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<PortadorReadDto> CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portadorModel = _portadorRepository.GetPortadorByCpf(portadorCreateDto.Cpf);

            if (portadorModel != null)
            {
                _portadorRepository.DeletePortador(portadorModel);
                _portadorRepository.SaveChanges();
            }

            portadorModel = _mapper.Map<Portador>(portadorCreateDto);

            _portadorRepository.CreatePortador(portadorModel);
            _portadorRepository.SaveChanges();

            return Ok(_mapper.Map<PortadorReadDto>(portadorModel));
        }

        [HttpDelete("{cpf}")]
        public ActionResult DeletePortador(string cpf)
        {
            var portadorModel = _portadorRepository.GetPortadorByCpf(cpf);

            if (portadorModel == null)
            {
                return NotFound();
            }

            var contas = _contasRepository.GetContasByCpfPortador(portadorModel.Cpf);

            if (contas.Count() > 0)
            {
                throw new Exception();
            }

            _portadorRepository.DeletePortador(portadorModel);
            _portadorRepository.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PortadorReadDto>> GetPortadores()
        {
            var portadores = _portadorRepository.GetPortadores();
            return Ok(_mapper.Map<IEnumerable<PortadorReadDto>>(portadores));
        }
    }
}