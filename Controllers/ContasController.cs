using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContasService.Data;
using ContasService.Dtos;
using ContasService.Models;
using Microsoft.AspNetCore.JsonPatch;

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

            if (contas.Count() == 0)
            {
                return NotFound();
            }

            _repository.DeleteContas(contas);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpGet("{numero}", Name="GetContaByNumero")]
        public ActionResult<ContaReadDto> GetContaByNumero(string numero)
        {
            var conta = _repository.GetContaByNumero(numero);

            if (conta == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ContaReadDto>(conta));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ContaReadDto>> GetContas()
        {
            var Contas = _repository.GetContas();
            return Ok(_mapper.Map<IEnumerable<ContaReadDto>>(Contas));
        }

        [HttpPatch("{numero}")]
        public ActionResult UpdateConta(string numero, JsonPatchDocument<ContaUpdateDto> patchDocument)
        {
            var conta = _repository.GetContaByNumero(numero);

            if (conta == null)
            {
                return NotFound();
            }

            var contaDto = _mapper.Map<ContaUpdateDto>(conta);
            patchDocument.ApplyTo(contaDto, ModelState);

            if(!TryValidateModel(contaDto))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(contaDto, conta);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}