namespace AutomacaoBancaria.Domain.Core.Models;

public class DataIncorretaException : Exception
{
    public DataIncorretaException(string mensagem) : base(mensagem)
    {
        
    }
}