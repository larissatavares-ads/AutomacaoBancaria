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
            var saldo = await _contaCorrenteRepository.ConsultarSaldo(agencia, conta);
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
    
    public async Task<ContaCorrente> ValidarContaCorrente(ContaCorrente contaCorrente)
    {
        await _contaCorrenteRepository.CriarContaCorrente(contaCorrente);
        return contaCorrente;
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
            var saldo = await _contaCorrenteRepository.ConsultarSaldo(agencia, conta);
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
}