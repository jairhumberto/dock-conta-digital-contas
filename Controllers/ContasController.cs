using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContasService.Data;
using ContasService.Dtos;
using ContasService.Models;

namespace ContasService.Controllers
{
    [Route("api/cs/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly IContasRepository _repository;
        private readonly IMapper _mapper;

        public ContasController(IContasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<ContaReadDto> CreateConta(ContaCreateDto ContaCreateDto)
        {
            var conta = _mapper.Map<Conta>(ContaCreateDto);
            
            _repository.CreateConta(conta);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetContaByNumero), new { Numero = conta.Numero }, _mapper.Map<ContaReadDto>(conta));
        }

        [HttpDelete("{portadorCpf}")]
        public ActionResult DeleteContasByCpfPortador(string portadorCpf)
        {
            var contas = _repository.GetContasByCpfPortador(portadorCpf);

            if (contas.Count() > 0)
            {
                _repository.DeleteContas(contas);
                _repository.SaveChanges();

                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("{numero}", Name="GetContaByNumero")]
        public ActionResult<ContaReadDto> GetContaByNumero(string numero)
        {
            var conta = _repository.GetContaByNumero(numero);

            if (conta != null)
            {
                return Ok(_mapper.Map<ContaReadDto>(conta));
            }

            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ContaReadDto>> GetContas()
        {
            var Contas = _repository.GetContas();
            return Ok(_mapper.Map<IEnumerable<ContaReadDto>>(Contas));
        }
    }
}