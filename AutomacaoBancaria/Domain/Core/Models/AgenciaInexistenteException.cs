namespace AutomacaoBancaria.Domain.Core.Models;

public class AgenciaInexistenteException : Exception
{
    public AgenciaInexistenteException(string mensagem) : base(mensagem)
    {
        
    }
}