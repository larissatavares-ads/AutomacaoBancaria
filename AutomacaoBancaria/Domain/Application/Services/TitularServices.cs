using System.Security.Cryptography.X509Certificates;
using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Interfaces.Application.Services;
using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Domain.Application.Services;

public class TitularServices : ITitularServices
{
    private ITitularRepository _titularRepository;
    public TitularServices(ITitularRepository titularRepository)
    {
        _titularRepository = titularRepository;
    }
    public async Task<Titular> FidelizarTitular(Titular titular)
    {
        await _titularRepository.CriarTitular(titular);
        return titular;
    }

    
    
    
    
    
    
    
    public bool ValidarCpf(string cpf)
    {
        string valor = cpf.Replace(".", "").Replace("-", "");
        if (valor.Length != 11)
            return false;
        return true;
    }
}