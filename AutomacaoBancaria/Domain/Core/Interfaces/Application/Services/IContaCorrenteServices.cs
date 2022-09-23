using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;

public interface IContaCorrenteServices
{
    Task ValidarContaCorrente(ContaCorrente contaCorrente);
    Task<ContaCorrente> ConsultarSaldo(int agencia, int conta);
    Task<ContaCorrente> RealizarDeposito(int agencia, int conta, decimal valorDeposito);
    Task RealizarSaque(int agencia, int conta, decimal valorSaque);
    Task LogCredito(int agencia, int conta, decimal valorDeposito);
    Task LogDebito(int agencia, int conta, decimal valorSaque);
}