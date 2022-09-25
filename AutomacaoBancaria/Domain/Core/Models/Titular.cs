namespace AutomacaoBancaria.Domain.Core.Models;

public class Titular
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public bool Erro { get; set; }
    public string Mensagem { get; set; }
}