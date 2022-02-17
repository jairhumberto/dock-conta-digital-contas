using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContasService.Data;
using ContasService.Dtos;
using ContasService.Models;
using Microsoft.AspNetCore.JsonPatch;
using ContasService.SyncDataServices.Http;

namespace ContasService.Controllers
{
    [Route("api/cs/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly IContasRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOperacaoDataClient _operacaoDataClient;

        public ContasController(IContasRepository repository, IMapper mapper, IOperacaoDataClient operacaoDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _operacaoDataClient = operacaoDataClient;
        }

        [HttpPost]
        public async Task<ActionResult<ContaReadDto>> CreateConta(ContaCreateDto ContaCreateDto)
        {
            var contaModel = _mapper.Map<Conta>(ContaCreateDto);

            _repository.CreateConta(contaModel);
            _repository.SaveChanges();
            
            try
            {
                var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
                await _operacaoDataClient.SendContaToOperacao(contaReadDto);
            }
            catch(Exception)
            {
                Console.WriteLine("Operacoes está indisponível");
            }

            return CreatedAtRoute(nameof(GetContaByNumero), new { Numero = contaModel.Numero }, _mapper.Map<ContaReadDto>(contaModel));
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