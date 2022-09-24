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
    [HttpGet("consultarExtrato/{agencia},{conta},{dataInicial},{dataFinal}")]
    public async Task<IActionResult> GetExtratoByContaCorrenteAsync(
        [FromRoute] int agencia,
        [FromRoute] int conta,
        [FromRoute] string dataInicial,
        [FromRoute] string dataFinal)
    {
        var titular = await _contaCorrenteServices.ConsultarExtrato(agencia,conta,dataInicial,dataFinal);
        
        //var texto = $"EXTRATO\r\nData Inicial ({dataInicial}) -- Data Final ({dataFinal})\r\n";
       
        
        return Ok(titular);
    }
    
    //consultar saldo
    [HttpGet("consultarSaldo/{agencia},{conta}")]
    public async Task<IActionResult> GetSaldoByContaCorrenteAsync(
        [FromRoute] int agencia,
        [FromRoute] int conta)
    {
        var titular = await _contaCorrenteServices.ConsultarSaldo(agencia,conta);
        return Ok(new {titular.Saldo});
    }
    
    //depósito conta corrente
    [HttpPut("realizarDeposito/{agencia},{conta},{valorDeposito}")]
    public async Task<IActionResult> PutDepositoAsync(
        [FromRoute] int agencia,
        [FromRoute] int conta,
        [FromRoute] decimal valorDeposito)
    {
        await _contaCorrenteServices.RealizarDeposito(agencia,conta,valorDeposito);
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
        await _contaCorrenteServices.RealizarSaque(agencia,conta,valorSaque);
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