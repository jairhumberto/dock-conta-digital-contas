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
        private readonly IContasRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOperacaoDataClient _operacaoDataClient;

        public OperacoesController(IContasRepository repository, IMapper mapper, IOperacaoDataClient operacaoDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _operacaoDataClient = operacaoDataClient;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessaOperacao(OperacaoReadDto operacaoReadDto)
        {
            var contaModel = _repository.GetContaByNumero(operacaoReadDto.ContaNumero);

            if (contaModel == null)
            {
                return NotFound("Conta nao encontrada");
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
            await _operacaoDataClient.SendContaToOperacao(contaReadDto);

            _repository.SaveChanges();

            return NoContent();
        }
    }
}