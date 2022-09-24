using System.Globalization;

namespace AutomacaoBancaria.Domain.Core.Models;

public class ContaCorrente
{
    public int Conta { get; set; }
    public int Agencia { get; set; }
    public decimal Saldo { get; set; }
    public string CpfTitular { get; set; }
    
    public Titular Titular { get; set; }
    public decimal Creditar(decimal valorDeposito)
    {
        var novoSaldo = Saldo += valorDeposito;
        return novoSaldo;
    }
    public decimal Debitar(decimal valor)
    {
        if(Saldo < valor)
                throw new SaldoInsuficienteException("Seu saldo é insuficiente para saque");
        var novoSaldo = Saldo -= valor;
        return novoSaldo;
    }
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
    public bool Erro { get; set; }
    public string Mensagem { get; set; }
}