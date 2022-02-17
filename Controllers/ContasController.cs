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
        private readonly IContasRepository _contasRepository;
        private readonly IPortadoresRepository _portadoresRepository;
        private readonly IMapper _mapper;
        private readonly IOperacaoDataClient _operacaoDataClient;

        public ContasController(IContasRepository contasRepository, IPortadoresRepository portadoresRepository,
                IMapper mapper, IOperacaoDataClient operacaoDataClient)
        {
            _contasRepository = contasRepository;
            _portadoresRepository = portadoresRepository;
            _mapper = mapper;
            _operacaoDataClient = operacaoDataClient;
        }

        [HttpPost]
        public async Task<ActionResult<ContaReadDto>> CreateConta(ContaCreateDto contaCreateDto)
        {
            var portadorModel = _portadoresRepository.GetPortadorByCpf(contaCreateDto.PortadorCpf);

            if (portadorModel == null)
            {
                return NotFound("Cpf nao cadastrado");
            }

            var contaModel = _mapper.Map<Conta>(contaCreateDto);
            contaModel.PortadorCpf = portadorModel.Cpf;

            _contasRepository.CreateConta(contaModel);
            _contasRepository.SaveChanges();
            
            try
            {
                var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
                await _operacaoDataClient.SendContaToOperacao(contaReadDto);
            }
            catch(Exception)
            {
                Console.WriteLine("Operacoes esta indisponivel");
            }

            return CreatedAtRoute(nameof(GetContaByNumero), new { Numero = contaModel.Numero }, _mapper.Map<ContaReadDto>(contaModel));
        }

        [HttpGet("{numero}", Name="GetContaByNumero")]
        public ActionResult<ContaReadDto> GetContaByNumero(string numero)
        {
            var contaModel = _contasRepository.GetContaByNumero(numero);

            if (contaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ContaReadDto>(contaModel));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ContaReadDto>> GetContas()
        {
            var contas = _contasRepository.GetContas();
            return Ok(_mapper.Map<IEnumerable<ContaReadDto>>(contas));
        }

        [HttpPatch("{numero}")]
        public async Task<ActionResult> UpdateConta(string numero, JsonPatchDocument<ContaUpdateDto> patchDocument)
        {
            var contaModel = _contasRepository.GetContaByNumero(numero);

            if (contaModel == null)
            {
                return NotFound();
            }

            var contaDto = _mapper.Map<ContaUpdateDto>(contaModel);
            patchDocument.ApplyTo(contaDto, ModelState);

            if(!TryValidateModel(contaDto))
            {
                return ValidationProblem(ModelState);
            }

            var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
            await _operacaoDataClient.SendContaToOperacao(contaReadDto);

            _mapper.Map(contaDto, contaModel);
            _contasRepository.SaveChanges();

            return NoContent();
        }
    }
}