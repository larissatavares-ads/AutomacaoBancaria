using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Interfaces.Application.Services;
using AutomacaoBancaria.Domain.Core.Models;
using AutomacaoBancaria.Ports.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AutomacaoBancaria.Ports.Controllers;

[ApiController]
[Route("titular")]
public class TitularController : ControllerBase
{
    private ITitularServices _titularServices;
    private IContaCorrenteServices _contaCorrenteServices;
    public TitularController(ITitularServices titularServices, IContaCorrenteServices contaCorrenteServices)
    {
        _titularServices = titularServices;
        _contaCorrenteServices = contaCorrenteServices;
    }
    
    //consultar extrato
    [HttpPost("consultarExtrato/{cpf}")]
    public async Task<IActionResult> GetByCpfAsync(
        [FromRoute] string cpf)
    {
        var titular = await _contaCorrenteServices.ConsultarExtrato(cpf);
        if (titular == null)
            return NotFound();
        return Ok(new {Saldo = titular.Saldo});
    }

    //criar usuario
    [HttpPost("validarTitular")]
    public async Task<IActionResult> PostAsync(
        [FromBody] TitularViewModel model)
    {
        var titular = new Titular
        {
            Nome = model.Nome,
            Cpf = model.Cpf
        };
        await _titularServices.FidelizarTitular(titular);
        return Ok(titular);
    }
    //Criar conta corrente
    [HttpPost("criarContaCorrente")]
    public async Task<IActionResult> PostAsync(
        [FromBody] ContaCorrenteViewModel model)
    {
        var contaCorrente = new ContaCorrente
        {
            Conta = model.Conta,
            Agencia = model.Agencia,
            Saldo = model.Saldo,
            CpfTitular = model.CpfTitular
        };
        await _contaCorrenteServices.ValidarContaCorrente(contaCorrente);
        return Ok(contaCorrente);
    }
}