namespace AutomacaoBancaria.Domain.Core.Models;

public class ContaInexistenteException : Exception
{
    public ContaInexistenteException(string mensagem) : base (mensagem)
    {
        
    }
}