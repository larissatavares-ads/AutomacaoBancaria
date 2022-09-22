namespace AutomacaoBancaria.Domain.Core.Models;

public class OperacaoFinanceiraException : Exception
{
    public OperacaoFinanceiraException(string mensagem, Exception excecaoInterna) : base(mensagem, excecaoInterna)
    {
    }
}