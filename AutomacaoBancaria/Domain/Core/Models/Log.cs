namespace AutomacaoBancaria.Domain.Core.Models;

public class Log
{
    public DateTime DataLog { get; set; } 
    public int CodigoLog { get; set; }
    public int AgenciaLog { get; set; }
    public int ContaLog { get; set; }
    public decimal ValorLog { get; set; }
}