using System.Data.SqlClient;
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
        try
        {
            await _titularRepository.CriarTitular(titular);
            return titular;
        }
        catch (Exception e)
        {
            var erro = new Titular();
            erro.Erro = true;
            erro.Mensagem = e.Message;
            Console.WriteLine(e);
            return erro;
        }
    }
}