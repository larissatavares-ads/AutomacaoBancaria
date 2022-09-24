namespace AutomacaoBancaria.Domain.Core.Models;

public class Log
{
    public string DataLog { get; set; }
    public int CodigoLog { get; set; }
    public int AgenciaLog { get; set; }
    public int ContaLog { get; set; }
    public decimal ValorLog { get; set; }
    public override string ToString()
    {
        if (CodigoLog == 1)
            return $"+{ValorLog}";
        return $"-{ValorLog}";
    }
    public bool Erro { get; set; }
    public string Mensagem { get; set; }
}