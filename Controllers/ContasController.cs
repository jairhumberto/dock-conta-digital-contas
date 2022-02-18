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
        private readonly IOperacoesServiceClient _operacoesServiceClient;

        public ContasController(IContasRepository contasRepository, IPortadoresRepository portadoresRepository,
                IMapper mapper, IOperacoesServiceClient operacoesServiceClient)
        {
            _contasRepository = contasRepository;
            _portadoresRepository = portadoresRepository;
            _mapper = mapper;
            _operacoesServiceClient = operacoesServiceClient;
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

            var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
            await _operacoesServiceClient.CreateConta(contaReadDto);

            _contasRepository.SaveChanges();

            return CreatedAtRoute(nameof(GetContaByNumero), new { Numero = contaModel.Numero },
                     _mapper.Map<ContaReadDto>(contaModel));
        }

        [HttpGet("{numero}", Name="GetContaByNumero")]
        public ActionResult<ContaReadDto> GetContaByNumero(string numero)
        {
            var contaModel = _contasRepository.GetContaByNumero(numero);

            if (contaModel == null)
            {
                return NotFound("Conta nao cadastrada");
            }

            return Ok(_mapper.Map<ContaReadDto>(contaModel));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ContaReadDto>> GetContas()
        {
            var contasModels = _contasRepository.GetContas();
            return Ok(_mapper.Map<IEnumerable<ContaReadDto>>(contasModels));
        }

        [HttpPatch("{numero}")]
        public async Task<ActionResult> UpdateConta(string numero, JsonPatchDocument<ContaUpdateDto> patchDocument)
        {
            var contaModel = _contasRepository.GetContaByNumero(numero);

            if (contaModel == null)
            {
                return NotFound("Conta nao cadastrada");
            }

            var contaDto = _mapper.Map<ContaUpdateDto>(contaModel);
            patchDocument.ApplyTo(contaDto, ModelState);

            if (!TryValidateModel(contaDto))
            {
                return ValidationProblem(ModelState);
            }
            
            _mapper.Map(contaDto, contaModel);
            
            var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
            await _operacoesServiceClient.CreateConta(contaReadDto);

            _contasRepository.SaveChanges();

            return NoContent();
        }
    }
}