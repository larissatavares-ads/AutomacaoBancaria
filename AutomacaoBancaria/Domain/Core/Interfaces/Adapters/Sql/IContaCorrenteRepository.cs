using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;

public interface IContaCorrenteRepository
{
    Task CriarContaCorrente(ContaCorrente contaCorrente);
    Task<ContaCorrente> ConsultarSaldo(int agencia, int conta);
    Task<ContaCorrente> ConsultarAgencia(int agencia);
}