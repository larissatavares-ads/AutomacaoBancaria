using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Core.Interfaces.Application.Services;

public interface ITitularServices
{
    Task<Titular> FidelizarTitular(Titular titular);
}