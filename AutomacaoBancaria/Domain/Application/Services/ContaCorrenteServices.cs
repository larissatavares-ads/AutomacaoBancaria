using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutomacaoBancaria.Domain.Application.Services;

public class ContaCorrenteServices : IContaCorrenteServices
{
    private IContaCorrenteRepository _contaCorrenteRepository;
    public ContaCorrenteServices(IContaCorrenteRepository contaCorrenteRepository)
    {
        _contaCorrenteRepository = contaCorrenteRepository;
    }
    public async Task ValidarContaCorrente(ContaCorrente contaCorrente)
    {
        await _contaCorrenteRepository.CriarContaCorrente(contaCorrente);
    }
    
    public async Task<ContaCorrente> ConsultarSaldo(int agencia, int conta)
    {
        try
        {
            var consultaAgencia = await _contaCorrenteRepository.ConsultarAgencia(agencia);
            if (consultaAgencia == null)
            {
                throw new AgenciaInexistenteException("Agência inexistente.");
            }
        }
        catch (AgenciaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
            {
                throw new ContaInexistenteException("Conta inexistente.");
            }
            return saldo;
        }
        catch (ContaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<List<Log>> ConsultarExtrato(int agencia, int conta, string dataInicial, string dataFinal)
    {
        var conversor = new Log();
        try
        {
            var consultaAgencia = await _contaCorrenteRepository.ConsultarAgencia(agencia);
            if (consultaAgencia == null)
                throw new AgenciaInexistenteException("Agência inexistente.");
        }
        catch (AgenciaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }
        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
                throw new ContaInexistenteException("Conta inexistente.");
        }
        catch (ContaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            
            var conversorDataInicial = conversor.ConversorDataInicial(dataInicial);
            var conversorDataFinal =  conversor.ConversorDataFinal(dataFinal);
            conversor.TestarData(conversorDataInicial, conversorDataFinal);
            
            
            var consultarExtrato = await _contaCorrenteRepository.ConsultarExtrato(agencia,conta,dataInicial,dataFinal);
            
            if (consultarExtrato == null)
                throw new DataIncorretaException("Data incorreta.");
            
            
            
            return consultarExtrato;
        }
        catch (DataIncorretaException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<ContaCorrente> RealizarDeposito(int agencia, int conta, decimal valorDeposito)
    {
        try
        {
            var consultaAgencia = await _contaCorrenteRepository.ConsultarAgencia(agencia);
            if (consultaAgencia == null)
            {
                throw new AgenciaInexistenteException("Agência inexistente.");
            }
        }
        catch (AgenciaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
            {
                throw new ContaInexistenteException("Conta inexistente.");
            }
            var novoSaldo = saldo.Creditar(valorDeposito);
            var efetivarDeposito = await _contaCorrenteRepository.EfetivarDeposito(agencia,conta,novoSaldo);
            return efetivarDeposito;
        }
        catch (ContaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task RealizarSaque(int agencia, int conta, decimal valorSaque)
    {
        try
        {
            var consultaAgencia = await _contaCorrenteRepository.ConsultarAgencia(agencia);
            if (consultaAgencia == null)
            {
                throw new AgenciaInexistenteException("Agência inexistente.");
            }
        }
        catch (AgenciaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }

        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
            {
                throw new ContaInexistenteException("Conta inexistente.");
            }
            var novoSaldo = saldo.Debitar(valorSaque);
            await _contaCorrenteRepository.EfetivarSaque(agencia,conta,novoSaldo);
        }
        catch (ContaInexistenteException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task LogCredito(int agencia, int conta, decimal valorDeposito)
    {
        var log = new Log
        {
            AgenciaLog = agencia,
            CodigoLog = 1,
            ContaLog = conta,
            ValorLog = valorDeposito
        };
        await _contaCorrenteRepository.LogCredito(log);
    }
    public async Task LogDebito(int agencia, int conta, decimal valorSaque)
    {
        var log = new Log
        {
            AgenciaLog = agencia,
            CodigoLog = 2,
            ContaLog = conta,
            ValorLog = valorSaque
        };
        await _contaCorrenteRepository.LogDebito(log);
    }
}