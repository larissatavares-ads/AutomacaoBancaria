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
    public async Task<ContaCorrente> ValidarContaCorrente(ContaCorrente contaCorrente)
    {
        await _contaCorrenteRepository.CriarContaCorrente(contaCorrente);
        return contaCorrente;
    }

    public async Task<ContaCorrente> ConsultarExtrato(string cpf)
    {
        try
        {
            var extrato = await _contaCorrenteRepository.ConsultarExtrato(cpf);
            return extrato;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}