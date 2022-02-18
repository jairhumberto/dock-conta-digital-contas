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
        private readonly IPortadoresRepository _portadoresRepository;
        private readonly IContasRepository _contasRepository;
        private readonly IMapper _mapper;

        public PortadoresController(IPortadoresRepository portadoresRepository, IContasRepository contasRepository,
                IMapper mapper)
        {
            _portadoresRepository = portadoresRepository;
            _contasRepository = contasRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portadorModel = _portadoresRepository.GetPortadorByCpf(portadorCreateDto.Cpf);

            if (portadorModel != null)
            {
                _portadoresRepository.DeletePortador(portadorModel);
            }

            _portadoresRepository.CreatePortador(_mapper.Map<Portador>(portadorCreateDto));
            _portadoresRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{cpf}")]
        public ActionResult DeletePortadorByCpf(string cpf)
        {
            var portadorModel = _portadoresRepository.GetPortadorByCpf(cpf);

            if (portadorModel == null)
            {
                return NotFound("Portador nao cadastrado");
            }

            var contasModel = _contasRepository.GetContasByCpfPortador(portadorModel.Cpf);

            if (contasModel.Count() > 0)
            {
                throw new HttpRequestException();
            }

            _portadoresRepository.DeletePortador(portadorModel);
            _portadoresRepository.SaveChanges();

            return NoContent();
        }
    }
}