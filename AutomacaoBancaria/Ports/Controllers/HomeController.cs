using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Interfaces.Application.Services;
using AutomacaoBancaria.Domain.Core.Models;
using AutomacaoBancaria.Ports.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AutomacaoBancaria.Ports.Controllers;

[ApiController]
[Route("titular")]
public class HomeController : ControllerBase
{
    private ITitularServices _titularServices;
    private IContaCorrenteServices _contaCorrenteServices;
    public HomeController(ITitularServices titularServices, IContaCorrenteServices contaCorrenteServices)
    {
        _titularServices = titularServices;
        _contaCorrenteServices = contaCorrenteServices;
    }
    
    //consultar extrato
    [HttpGet("consultarExtrato")]
    public async Task<IActionResult> GetExtratoByContaCorrenteAsync(
        [FromQuery] int agencia,
        [FromQuery] int conta,
        [FromQuery] string dataInicial,
        [FromQuery] string dataFinal)
    {
        var lista = await _contaCorrenteServices.ConsultarExtrato(agencia,conta,dataInicial,dataFinal);
        var extrato = lista.Select(log => new {log.DataLog, ValorLog = log.ToString() });
        var erro = lista.Select(log => new {log.Erro, log.Mensagem}).FirstOrDefault();
        if (erro.Erro)
            return BadRequest(new { erro.Mensagem });
        return Ok(extrato);
    }
    
    //consultar saldo
    [HttpGet("consultarSaldo")]
    public async Task<IActionResult> GetSaldoByContaCorrenteAsync(
        [FromQuery] int agencia,
        [FromQuery] int conta)
    {
        var saldo = await _contaCorrenteServices.ConsultarSaldo(agencia,conta);
        if (saldo.Erro)
            return BadRequest(new {saldo.Mensagem});
        return Ok(new {saldo.Saldo});
    }
    
    //depósito conta corrente
    [HttpPut("realizarDeposito/{agencia},{conta},{valorDeposito}")]
    public async Task<IActionResult> PutDepositoAsync(
        [FromRoute] int agencia,
        [FromRoute] int conta,
        [FromRoute] decimal valorDeposito)
    {
        var deposito = await _contaCorrenteServices.RealizarDeposito(agencia,conta,valorDeposito);
        if (deposito.Erro)
            return BadRequest(new {deposito.Mensagem});
        await _contaCorrenteServices.LogCredito(agencia,conta,valorDeposito);
        return Ok("Depósito realizado com sucesso.");
    }
    
    //saque conta corrente
    [HttpPut("realizarSaque/{agencia},{conta},{valorSaque}")]
    public async Task<IActionResult> PutSaqueAsync(
        [FromRoute] int agencia,
        [FromRoute] int conta,
        [FromRoute] decimal valorSaque)
    {
        var saque = await _contaCorrenteServices.RealizarSaque(agencia,conta,valorSaque);
        if (saque.Erro)
            return BadRequest(new {saque.Mensagem});
        await _contaCorrenteServices.LogDebito(agencia,conta,valorSaque);
        return Ok("Saque realizado com sucesso.");
    }
    
    //criar titular
    [HttpPost("criarTitular")]
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
    
    //criar conta corrente
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
        return Ok("Conta Corrente criada com sucesso.");
    }
}