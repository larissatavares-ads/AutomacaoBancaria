using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;

public interface ITitularRepository
{
    Task CriarTitular(Titular titular);
}