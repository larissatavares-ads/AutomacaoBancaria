using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;

public interface IContaCorrenteServices
{
    Task<ContaCorrente> ValidarContaCorrente(ContaCorrente contaCorrente);
    Task<ContaCorrente> ConsultarSaldo(int agencia, int conta);
    Task<ContaCorrente> RealizarDeposito(int agencia, int conta, decimal valorDeposito);
}