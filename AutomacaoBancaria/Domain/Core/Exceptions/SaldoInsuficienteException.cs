namespace AutomacaoBancaria.Domain.Core.Models;

public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException(string mensagem) : base(mensagem)
    {
        
    }
}