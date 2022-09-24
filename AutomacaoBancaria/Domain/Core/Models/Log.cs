using System.Globalization;

namespace AutomacaoBancaria.Domain.Core.Models;

public class Log
{
    public DateTime DataConsulta { get; set; } 
    public int CodigoLog { get; set; }
    public int AgenciaLog { get; set; }
    public int ContaLog { get; set; }
    public decimal ValorLog { get; set; }
    
    public DateTime ConversorDataInicial(string dataInicial)
    {
        var culture = new CultureInfo("pt-BR");
        var data = DateTime.Parse(dataInicial, culture);
        return data;
    }
    public DateTime ConversorDataFinal(string dataFinal)
    {
        var culture = new CultureInfo("pt-BR");
        var data = DateTime.Parse(dataFinal, culture);
        return data;
    }
    public void TestarData(DateTime dataInicial, DateTime dataFinal)
    {
        if (dataInicial > dataFinal)
            throw new DataIncorretaException("Data final deve ser maior ou igual a data inicial");
    }
}