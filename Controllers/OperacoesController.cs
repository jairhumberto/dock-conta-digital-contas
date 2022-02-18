using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ContasService.Data;
using ContasService.Dtos;
using ContasService.SyncDataServices.Http;

namespace ContasService.Controllers
{
    [Route("api/cs/[controller]")]
    [ApiController]
    public class OperacoesController : ControllerBase
    {
        private readonly IContasRepository _contasRepository;
        private readonly IMapper _mapper;
        private readonly IOperacoesServiceClient _operacoesServiceClient;

        public OperacoesController(IContasRepository contasRepository, IMapper mapper,
                IOperacoesServiceClient operacoesServiceClient)
        {
            _contasRepository = contasRepository;
            _mapper = mapper;
            _operacoesServiceClient = operacoesServiceClient;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessaOperacao(OperacaoReadDto operacaoReadDto)
        {
            var contaModel = _contasRepository.GetContaByNumero(operacaoReadDto.ContaNumero);

            if (contaModel == null)
            {
                return NotFound("Conta nao cadastrada");
            }

            if (operacaoReadDto.Valor <= 0)
            {
                return BadRequest("Valor invalido");
            }
            
            if (operacaoReadDto.Tipo == "saque" && contaModel.Saldo < operacaoReadDto.Valor)
            {
                return Unauthorized("Saldo insuficiente");
            }

            if (!contaModel.Ativa || contaModel.Bloqueada)
            {
                return Unauthorized("Conta inativa ou bloqueada");
            }

            contaModel.Saldo += operacaoReadDto.Tipo == "saque" ? -operacaoReadDto.Valor : operacaoReadDto.Valor;
            
            var contaReadDto = _mapper.Map<ContaReadDto>(contaModel);
            await _operacoesServiceClient.CreateConta(contaReadDto);

            _contasRepository.SaveChanges();

            return NoContent();
        }
    }
}