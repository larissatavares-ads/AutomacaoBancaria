using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Models;

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
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Agência inexistente";
            Console.WriteLine(e);
            return erro;
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
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Conta inexistente";
            Console.WriteLine(e);
            return erro;
        }
    }
    public async Task<List<Log>> ConsultarExtrato(int agencia, int conta, string dataInicial, string dataFinal)
    {
        try
        {
            var consultaAgencia = await _contaCorrenteRepository.ConsultarAgencia(agencia);
            if (consultaAgencia == null)
                throw new AgenciaInexistenteException("Agência inexistente.");
        }
        catch (AgenciaInexistenteException e)
        {
            var ex = new List<Log>
            {
                new Log
                {
                    Erro = true,
                    Mensagem = "Agencia inexistente"
                }
            };
            Console.WriteLine(e);
            return ex;
        }

        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
                throw new ContaInexistenteException("Conta inexistente.");

            var conversorDataInicial = saldo.ConversorDataInicial(dataInicial);
            var conversorDataFinal = saldo.ConversorDataFinal(dataFinal);
            saldo.TestarData(conversorDataInicial, conversorDataFinal);

            var consultarExtrato =
                await _contaCorrenteRepository.ConsultarExtrato(agencia, conta, conversorDataInicial,
                    conversorDataFinal);

            if (consultarExtrato == null)
                throw new DataIncorretaException("Data incorreta.");
            return consultarExtrato;
        }
        catch (ContaInexistenteException e)
        {
            var ex = new List<Log>
            {
                new Log
                {
                    Erro = true,
                    Mensagem = "Conta inexistente"
                }
            };
            Console.WriteLine(e);
            return ex;
        }
        catch (DataIncorretaException e)
        {
            var ex = new List<Log>
            {
                new Log
                {
                    Erro = true,
                    Mensagem = "Data final deve ser maior ou igual a data inicial"
                }
            };
            Console.WriteLine(e);
            return ex;
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
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Agência inexistente";
            Console.WriteLine(e);
            return erro;
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
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Conta inexistente";
            Console.WriteLine(e);
            return erro;
        }
    }
    public async Task<ContaCorrente> RealizarSaque(int agencia, int conta, decimal valorSaque)
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
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Agência inexistente";
            Console.WriteLine(e);
            return erro;
        }
        try
        {
            var saldo = await _contaCorrenteRepository.ConsultarConta(agencia, conta);
            if (saldo == null)
            {
                throw new ContaInexistenteException("Conta inexistente.");
            }

            var novoSaldo = saldo.Debitar(valorSaque);
            var saque = await _contaCorrenteRepository.EfetivarSaque(agencia, conta, novoSaldo);
            return saque;
        }
        catch (ContaInexistenteException e)
        {
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Conta inexistente";
            Console.WriteLine(e);
            return erro;
        }
        catch (SaldoInsuficienteException e)
        {
            var erro = new ContaCorrente();
            erro.Erro = true;
            erro.Mensagem = "Seu saldo é insuficiente para saque.";
            Console.WriteLine(e);
            return erro;
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